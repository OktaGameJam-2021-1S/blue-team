using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIViewManager : MonoBehaviour
{
   public Dictionary<Type, IView> Screens = new Dictionary<Type, IView>();
   
   public ViewData ViewData;
    
   public ViewData PopUpViewData;
   
   public T GetScreen<T>()
   {
      return (T)Screens[typeof(T)];
   }

   public void RegisterAllViews()
   {
      Screens.Clear();

      var scene = SceneManager.GetActiveScene();
      var objs = scene.GetRootGameObjects();
      var views = FindObjectsOfType<MonoBehaviour>().OfType<IView>();
      foreach (var obj in objs)
      {
         views = obj.GetComponentsInChildren<IView>(true);
         foreach (var view in views)
         {
            RegisterScreen(view);
         }
      }
   }
   private void RegisterScreen(IView screen)
   {
      Screens.Add(screen.GetType(), screen);
   }
}
