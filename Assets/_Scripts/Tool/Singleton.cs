using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
   private static T instance;
   private static bool dontDetroyOnLoad;
   public static T Instance
   {
      get
      {
         // Thực hiện kiểm tra xem instance nó có null không
         if (instance == null)
         {
            instance = FindObjectOfType<T>(true);
            // Kiểm tra tieeps nếu nó còn null 
            if (instance == null)
            {
               GameObject singleObject = new GameObject();
               instance = singleObject.AddComponent<T>();
               singleObject.name = typeof(T).Name + "-singleton";
               if (dontDetroyOnLoad)
               {
                  DontDestroyOnLoad(singleObject);
               }
            }
         }
         return instance;
      }
   }

   protected virtual void KeepAlive(bool enale)
   {
      dontDetroyOnLoad = enale;
   }
   protected virtual void Awake()
   {
      if (instance != null && instance.GetInstanceID() != GetInstanceID())
      {
         Destroy(this);
         return;
      }
      instance = (T) (MonoBehaviour) this;
      if (dontDetroyOnLoad)
      {
         DontDestroyOnLoad(this);
      }
   }
}
