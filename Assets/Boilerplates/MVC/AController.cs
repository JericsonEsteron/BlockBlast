using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MVC
{
    [DefaultExecutionOrder(-10)]
    public abstract class AController<V, M> : MonoBehaviour, IController where V : AView<M> where M : IModel, new()
    {
        private V _view;
        private M _model;

        protected abstract void OnModelBound(M model);
        protected abstract void OnModelUnBound(M model);

        protected virtual void Awake()
        {
            _model = CreateModel();
            _view = FindView();

            _view.Bind(_model);
        }
        
        protected virtual void OnEnable() 
        {
            OnModelBound(_model);
        }

        protected virtual void OnDisable() 
        {
            OnModelUnBound(_model);
        }

        private M CreateModel()
        {
            return new M();
        }

        private V FindView()
        {
            return GetComponent<V>();
        }
    }

    public abstract class AController<M> : MonoBehaviour, IController where M : IModel, new()
    {
        private M _model;
        
        public M Model => _model;

        protected abstract void OnModelBound();
        protected abstract void OnModelUnBound();
        
        protected void OnEnable() 
        {
            Bind();
        }

        protected void OnDisable() 
        {
            UnBind();
        }

        private void Bind()
        {
            if(_model != null)
                return;

            _model = CreateModel();
            OnModelBound();
        }

        private void UnBind()
        {
            if(_model == null)
                return;

            _model = default;
            OnModelUnBound();
        }

        private M CreateModel()
        {
            return new M();
        }
    }

}
