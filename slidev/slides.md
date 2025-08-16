---
title: IDictionary to caching nirvana
drawings:
  persist: false
transition: slide-left
theme: dracula
layout: image-right
image: 4P.jpg
---

# IDictionary to caching nirvana
## Who am I?
Christoph Donges
cdonges@gmail.com

- Senior Software Engineer at Vald

---
class: cover
drawings:
  persist: false
transition: slide-left
---

# The events you are about to witness are __true__. The names and locations have been changed to protect those still living.

---
layout: image-right
image: milton.jpg
transition: slide-left
---

# A developer
# Let's call him Milton
- Gets tasked to add a dashboard to the TPS reports
- Creates a SQL query to get the data
- The data is good but when it goes to prod the app is slow and the database is overloaded

What to do?

---
transition: slide-left
---

# IDictionary
- Simple
- Things are faster

&nbsp;

``` cs
if (!dictionary.ContainsKey(key))
{
    dictionary[key] = await func();
}

return dictionary[key];

// registration
serviceCollection.AddSingleton<ICacheService, DictionaryCacheService>();
```
---
layout: image
image: dictionary1.png
backgroundSize: contain
transition: slide-left
---

# IDictionary

---
layout: image-right
image: milton1.webp
transition: slide-left
---
# IDictionary
## However
- Getting interesting threading errors

---
transition: slide-left
---

# ConcurrentDictionary
- Fixes concurrent issues

&nbsp;

``` cs
if (!dictionary.ContainsKey(key))
{
    dictionary[key] = await func();
}

return dictionary[key];

// registration
serviceCollection.AddSingleton<ICacheService, ConcurrentDictionaryCacheService>();
```
---
layout: image-right
image: milton2.jpg
transition: slide-left
---
# ConcurrentDictionary
## However
- Size grows

---
transition: slide-left
---
# IMemoryCache
- Evicts older entries
- This is the first 'cache' I would recommend

&nbsp;

``` cs
return (await memoryCache.GetOrCreateAsync(key, async key => await func()))!;

// registration
serviceCollection.AddMemoryCache().AddSingleton<ICacheService, MemoryCacheService>();
```
---
layout: image
image: memoryCache1.png
backgroundSize: contain
transition: slide-left
---
# IMemoryCache

---
layout: image-right
image: milton3.jpg
transition: slide-left
---
# IMemoryCache
## However
- Duplicated calls to SQL
- Cache invalidation

---
transition: slide-left
---

# IDistributedCache
- Removes duplicated calls
- Invalidation easy
- Redis, SQL Server, NCache + Others

``` cs
var bytes = await distributedCache.GetAsync(key);
if (bytes == null)
{
    string val = await func();
    await distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(val));
    return val;
}
return Encoding.UTF8.GetString(bytes);

// registration
serviceCollection
    .AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "localhost";
        options.InstanceName = "sample";
    })
    .AddSingleton<ICacheService, DistributedCacheService>();
```

---
layout: image
image: distributedCache1.png
backgroundSize: contain
transition: slide-left
---
# IDistributedCache
---
layout: image-right
image: milton4.webp
transition: slide-left
---
# IDistributedCache
## However
- Slower to get value
- Cost and load on distributed cache

---
transition: slide-left
---

# HybridCache
- Two levels of caching
- Stampede protection

&nbsp;

``` cs
return await hybridCache.GetOrCreateAsync(
    key,
    async cancel => await func());

// registration
serviceCollection.AddHybridCache();
serviceCollection
    .AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "localhost";
        options.InstanceName = "sample";
    })
    .AddMemoryCache()
    .AddSingleton<ICacheService, HybridCacheService>();
```
---
layout: image
image: hybridCache1.png
backgroundSize: contain
transition: slide-left
---
# HybridCache
---
layout: image-right
image: milton5.jpg
transition: slide-left
---
# HybridCache
## However
- Local caches can get out of date

---
transition: slide-left
image: bat.png
layout: image-right
---

# Stampede protection
- Multiple requests for the same data at the same time
- Only retrieves the first requests while the others wait
- Reduce resource usage
- Included in HybridCache, with nothing to do

---
layout: image-right
image: mulecache_logo.jpg
transition: slide-left
---

# MuleCache
- An implementation of HybridCache
- Company hack-a-thon project
- Adds distributed cache invalidation using Redis pub/sub
- Created a logo, which is the hardest part of any project

&nbsp;

---
layout: image-right
image: milton.jpg
transition: slide-left
---
# MuleCache
## However
- There is a project that already does all this (and more).

---
transition: slide-left
---

# FusionCache
- Fast retrieval from local, Secondary load from distributed
- Distributed invalidation with redis pub/sub called backplane
- Stampede protection
- Adaptive

``` cs
return await fusionCache.GetOrSetAsync(
    key,
    async cancel => await func());
// registration
serviceCollection
    .AddFusionCache()
    .WithSerializer(
        new FusionCacheSystemTextJsonSerializer()
    )
    .WithDistributedCache(
        new RedisCache(new RedisCacheOptions { Configuration = "localhost" })
    )
    .WithBackplane(
        new RedisBackplane(new RedisBackplaneOptions { Configuration = "localhost" })
    );

serviceCollection.AddSingleton<ICacheService, FusionCacheService>();
```
&nbsp;

---
layout: image
image: fusionCache1.png
backgroundSize: contain
transition: slide-left
---
# FusionCache

---
transition: slide-left
layout: image-right
image: milton-cake.jpg
---

# Adaptave caching
- Things like auth tokens
- Don't know what the expiry should be until you get the value

``` cs
   async (context, _) =>
    {
        var token = await GetNewAccessToken();
        context.Options.Duration = 
            TimeSpan.FromSeconds(token.ExpiresIn);
        return token;
    })).AccessToken;
```

---
transition: slide-left
image: lumberg.jpg
layout: image-right
---

# Questions?

---
transition: slide-left
layout: image-right
image: qrcode.png
---

# Thank you
## Get the code and slides here
[https://github.com/cdonges/ddd-outback-2025](https://github.com/cdonges/ddd-outback-2025)

## Other useful links
HybridCache - [https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid?view=aspnetcore-9.0](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/hybrid?view=aspnetcore-9.0)

FusionCache - [https://github.com/ZiggyCreatures/FusionCache](https://github.com/ZiggyCreatures/FusionCache)

Office Space - [https://en.wikipedia.org/wiki/Office_Space](https://en.wikipedia.org/wiki/Office_Space)
