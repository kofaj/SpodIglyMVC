using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpodIglyMVC.Infrastructure
{
    public interface ICasheProvider
    {
        object Get(string key);
        void Set(string key, object data, int casheTime);
        bool IsSet(string key);
        void Invalidate(string key);
    }
}