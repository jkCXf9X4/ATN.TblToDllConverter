# Catia.R24

Converts and merges catia TBLs into a single dll

Adds a singelton CatiaApplication for direct COM access


```c#
using Dassault.Catia.R24.INFITF;

using ATN.Catia.R24.COM;
//
Application application = CatiaApplication.Instance;

```

## Credits

Uses IlMerge to wrap the dlls into a single lib






