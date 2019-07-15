using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoX
{
    /// <summary>
    /// 
    /// </summary>
    public struct TryResult
    {

        public TryResult(IEnumerable<Exception> exceptions)
        {
            if (exceptions != null && exceptions.Any())
            {
                _hasError = true;
                this._error = exceptions.First();
                this._errors = exceptions.ToArray();
            }
            else
            {
                _hasError = false;
                this._error = null;
                this._errors = null;
            }
        }

        public TryResult(params Exception[] exceptions)
        {

            if (exceptions != null && exceptions.Length > 0)
            {
                _hasError = true;
                this._error = exceptions[0];
                this._errors = exceptions;
            }
            else
            {
                _hasError = false;
                this._error = null;
                this._errors = null;
            }
        }

        public TryResult(bool flag)
        {
            _hasError = !flag;
            this._error = null;
            this._errors = null;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool HasError
        {
            get { return _hasError; }
        }
        private readonly Exception _error;
        private readonly bool _hasError;
        private Exception[] _errors;

        public Exception Error
        {
            get { return _error; }
        }

        public Exception[] Errors
        {
            get { return _errors; }

        }
        public static implicit operator TryResult(Exception ex)
        {
            return new TryResult(ex);
        }
        public static implicit operator TryResult(bool flag)
        {
            return new TryResult(flag);
        }
        public static implicit operator bool(TryResult value)
        {
            return !value.HasError;
        }

        public static bool operator true(TryResult x)
        {

            return !x.HasError;
        }
        public static bool operator false(TryResult x)
        {
            return x.HasError;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct TryResult<T>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public TryResult(T value)
        {
            _value = value;
            _hasError = false;
            _error = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public TryResult(Exception exception)
            : this(exception, default(T))
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="defaultValue"></param>
        public TryResult(Exception exception, T defaultValue)
        {
            _value = defaultValue;
            _hasError = exception!=null;
            this._error = exception;
        }
        private readonly T _value;
        private readonly bool _hasError;
        private readonly Exception _error;
        public Exception Error
        {
            get { return _error; }

        }



        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get { return _value; }

        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasError
        {
            get { return _hasError; }

        }

        public static implicit operator TryResult<T>(T value)
        {
            return new TryResult<T>(value);
        }
        public static implicit operator TryResult<T>(Exception ex)
        {
            return new TryResult<T>(ex);
        }

        public static implicit operator T(TryResult<T> value)
        {
            return value.Value;
        }
        public static bool operator true(TryResult<T> x)
        {
            return !x.HasError;
        }
        public static bool operator false(TryResult<T> x)
        {
            return x.HasError;
        }

        public static TryResult<TModel> Cast<TModel>(TryResult<object> obj)
        {
            if (obj.HasError) return obj.Error;
            if (obj.Value is TModel == false)
                return new Exception(String.Format("Type Not Same [{0}] [{1}]", obj.Value.GetType().FullName, typeof(TModel).FullName));
            return (TModel)obj;
        }

        public static TryResult<TModel> Cast<TModel>(object obj)
        {
            if (obj == null) return new TryResult<TModel>();
            if (obj is TModel == false)
                return new Exception(String.Format("Type Not Same [{0}] [{1}]", obj.GetType().FullName, typeof(TModel).FullName));
            return new TryResult<TModel>((TModel)obj);
        }
    }
}