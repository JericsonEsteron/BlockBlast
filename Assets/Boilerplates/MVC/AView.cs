using System.Collections;
using System.Collections.Generic;
using MVC;
using UnityEngine;

namespace MVC
{
    public abstract class AView<M> : MonoBehaviour, IView where M : IModel
    {
        private M _model;

        protected abstract void OnModelBound(M model);
        protected abstract void OnModelUnBound(M model);

        protected virtual void OnDestroy() 
        {
            UnBind();
        }

        public void Bind(M model)
        {
            _model = model;
            OnModelBound(_model);
        }

        public void UnBind()
        {
            _model = default;
            OnModelUnBound(_model);
        }
    }

}
