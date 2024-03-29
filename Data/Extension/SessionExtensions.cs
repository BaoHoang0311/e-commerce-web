﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.Extension
{
    public static class SessionExtensions
    {
        public static void Sets<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T Gets<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) 
                :JsonConvert.DeserializeObject<T>(value);
        }
    }
}