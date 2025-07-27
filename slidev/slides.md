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
class: text-center
drawings:
  persist: false
transition: slide-left
---

# The events you are about to witness are __true__.

# The names and locations have been changed to protect those still living.

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

# However
- Getting interesting threading errors

---
transition: slide-left
---
# ConcurrentDictionary
- Fixes concurrent issues

# However
- Size grows
---
transition: slide-left
---
# IMemoryCache
- Evicts older entries

# However
- Duplicated calls to resource
- Cache invalidation
---
transition: slide-left
---
# IDistributedCache
- Removes duplicaed calles
- Incalidation easy

# However
- Slower to get value
- Cost and load on distibuted cache
---
transition: slide-left
---
# HybridCache
- Cache invalidation

# However
- Cache invalidation
---
transition: slide-left
---
# MuleCache
- An experement
---
transition: slide-left
---
# FusionCache
- Fast retrieval from local
- Secondary load from distributed
- Distributed invalidation
- Adaptive
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
