---
title: IDictionary to caching nirvana
drawings:
  persist: false
transition: slide-left
theme: dracula
---

# IDictionary to caching nirvana
## Who am I?
Christoph Donges
cdonges@gmail.com

Senior Softare Engineer at Vald

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
```
&nbsp;

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
```

&nbsp;

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
```

&nbsp;

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
```

## However
- Slower to get value
- Cost and load on distibuted cache

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
```
&nbsp;

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
- An implementation of HbridCache
- Company hack-a-thon project
- Adds distributed cache invalidation using Redis pub/sub
- Created a logo, which is the hardest part of any project

&nbsp;

## However
- There is a project that already does all this (and more).

---
transition: slide-left
---

# FusionCache
- Fast retrieval from local
- Secondary load from distributed
- Distributed invalidation
- Stampede protection
- Adaptive

&nbsp;

``` cs
return await fusionCache.GetOrSetAsync(
    key,
    async cancel => await func());
```

---
transition: slide-left
image: tps.jpg
layout: image-right
---

# Code demo
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
