using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Cession.Commands
{
    public static class CollectionExtension
    {
        public static void AddRange<T> (this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add (item);
        }
    }

    public partial class Command
    {
        #region "utility methods for helping create commands"

        public static Command Create(Action action)
        {
            return new Command0 (action, action);
        }

        public static Command Create(Action executeAction,Action undoAction)
        {
            return new Command0 (executeAction, undoAction);
        }

        public static Command CreateClear<T> (ICollection<T> collection)
        {
            var command = new Command<ICollection<T>,T[]> (collection, collection.ToArray (),
                (c, a) => c.Clear (),
                (c, a) => c.AddRange (a));
            return command;
        }

        public static Command CreateDictionaryAdd<TKey,TValue> (IDictionary<TKey,TValue> dictionary,
            TKey key,
            TValue value)
        {
            var command = new Command<IDictionary<TKey,TValue>,TKey,TValue> (dictionary,
                key,
                value,
                (d, k, v) => d.Add (key, value),
                (d, k, v) => d.Remove (k));
            return command;
        }

        public static Command CreateDictionaryRemove<TKey,TValue> (IDictionary<TKey,TValue> dictionary,
            TKey key)
        {
            var command = new Command<IDictionary<TKey,TValue>,TKey,TValue> (dictionary,
                key,
                dictionary [key],
                (d, k, v) => d.Remove (k),
                (d, k, v) => d.Add (k, v));
            return command;
        }

        public static Command CreateDictionaryReplace<TKey,TValue> (IDictionary<TKey,TValue> dictionary,
            TKey key,
            TValue value)
        {
            return Create<IDictionary<TKey,TValue>,TKey,TValue> (dictionary, 
                key, 
                value, 
                dictionary [key],
                (d, k, v) => d [k] = v);
        }

        public static Command CreateListInsert<T> (IList<T> list, int index, T value)
        {
            var command = new Command<IList<T> ,int,T> (list, index, value, 
                (l, i, v) => l.Insert (i, v),
                (l, i, v) => l.RemoveAt (i));
            return command;
        }

        public static Command CreateListAdd<T> (IList<T> list, T value)
        {
            return CreateListInsert (list, list.Count, value);
        }

        public static Command CreateListRemove<T> (IList<T> list, int index)
        {
            return new Command<IList<T>,int,T> (list, index,
                list [index],
                (l, i, v) => l.RemoveAt (i),
                (l, i, v) => l.Insert (i, v));
        }

        public static Command CreateListRemove<T> (IList<T> list, T item)
        {
            return CreateListRemove (list, list.IndexOf (item));
        }

        public static Command CreateListReplace<T> (IList<T> list, int index, T value)
        {
            return Create<IList<T>,int,T> (list, index, value, list [index],
                (l, i, v) => l [i] = v);
        }

        public static Command Create<T> (T newValue, T oldValue, Action<T> action)
        {
            return new Command<T,T> (newValue, oldValue,
                (nv, ov) => action.Invoke (nv),
                (nv, ov) => action.Invoke (ov));
        }

        public static Command Create<T1,T2> (T1 target, T2 newValue, T2 oldValue, Action<T1,T2> action)
        {
            return new Command<T1,T2,T2> (target, newValue, oldValue,
                (t, nv, ov) => action.Invoke (t, nv),
                (t, nv, ov) => action.Invoke (t, ov));
        }

        public static Command Create<T1,T2,T3> (T1 target,
            T2 index,
            T3 newValue,
            T3 oldValue,
            Action<T1,T2,T3> action)
        {
            return new Command<T1,T2,T3,T3> (target, index, newValue, oldValue,
                (l, i, nv, ov) => action.Invoke (target, i, nv),
                (l, i, nv, ov) => action.Invoke (target, i, ov));
        }

        public static Command CreateSetProperty<T1,T2> (T1 target,
            T2 value,
            string propertyName)
        {
            var type = typeof(T1);
            var pi = type.GetRuntimeProperty (propertyName);
            if (null == pi)
                throw new ArgumentException (
                    string.Format ("can't find property {0} on type {1} or this property is readonly", 
                        propertyName, 
                        type.Name));

            var oldValue = (T2)pi.GetValue (target, null);
            Action<T2> action = v => pi.SetValue (target, v);
            return Create (value, oldValue, action);
        }

        #endregion
    }
}

