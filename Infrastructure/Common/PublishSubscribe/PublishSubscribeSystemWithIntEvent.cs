using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScriptableFramework
{
    internal interface IDelegation
    {
        System.Reflection.ParameterInfo[] Parameters { get; }
        Delegate Action { get; }
        void Execute(IList<BoxedValue> list);
    }
    internal abstract class AbstractDelegation : IDelegation
    {
        public System.Reflection.ParameterInfo[] Parameters
        {
            get {
                if (null == m_Parameters && null != Action)
                    m_Parameters = Action.Method.GetParameters();
                return m_Parameters;
            }
        }
        public abstract Delegate Action { get; }
        public abstract void Execute(IList<BoxedValue> list);

        private System.Reflection.ParameterInfo[] m_Parameters = null;
    }
    internal class GenericDelegation : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action) {
                m_Action();
            }
        }
        public MyAction m_Action;
    }
    internal class GenericDelegation<T1> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 1) {
                var p1 = list[0].CastTo<T1>();
                m_Action(p1);
            }
        }
        public MyAction<T1> m_Action;
    }
    internal class GenericDelegation<T1, T2> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 2) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                m_Action(p1, p2);
            }
        }
        public MyAction<T1, T2> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 3) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                m_Action(p1, p2, p3);
            }
        }
        public MyAction<T1, T2, T3> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3, T4> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 4) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                var p4 = list[3].CastTo<T4>();
                m_Action(p1, p2, p3, p4);
            }
        }
        public MyAction<T1, T2, T3, T4> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3, T4, T5> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 5) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                var p4 = list[3].CastTo<T4>();
                var p5 = list[4].CastTo<T5>();
                m_Action(p1, p2, p3, p4, p5);
            }
        }
        public MyAction<T1, T2, T3, T4, T5> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3, T4, T5, T6> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 6) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                var p4 = list[3].CastTo<T4>();
                var p5 = list[4].CastTo<T5>();
                var p6 = list[5].CastTo<T6>();
                m_Action(p1, p2, p3, p4, p5, p6);
            }
        }
        public MyAction<T1, T2, T3, T4, T5, T6> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3, T4, T5, T6, T7> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 7) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                var p4 = list[3].CastTo<T4>();
                var p5 = list[4].CastTo<T5>();
                var p6 = list[5].CastTo<T6>();
                var p7 = list[6].CastTo<T7>();
                m_Action(p1, p2, p3, p4, p5, p6, p7);
            }
        }
        public MyAction<T1, T2, T3, T4, T5, T6, T7> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 8) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                var p4 = list[3].CastTo<T4>();
                var p5 = list[4].CastTo<T5>();
                var p6 = list[5].CastTo<T6>();
                var p7 = list[6].CastTo<T7>();
                var p8 = list[7].CastTo<T8>();
                m_Action(p1, p2, p3, p4, p5, p6, p7, p8);
            }
        }
        public MyAction<T1, T2, T3, T4, T5, T6, T7, T8> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8, T9> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 9) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                var p4 = list[3].CastTo<T4>();
                var p5 = list[4].CastTo<T5>();
                var p6 = list[5].CastTo<T6>();
                var p7 = list[6].CastTo<T7>();
                var p8 = list[7].CastTo<T8>();
                var p9 = list[8].CastTo<T9>();
                m_Action(p1, p2, p3, p4, p5, p6, p7, p8, p9);
            }
        }
        public MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 10) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                var p4 = list[3].CastTo<T4>();
                var p5 = list[4].CastTo<T5>();
                var p6 = list[5].CastTo<T6>();
                var p7 = list[6].CastTo<T7>();
                var p8 = list[7].CastTo<T8>();
                var p9 = list[8].CastTo<T9>();
                var p10 = list[9].CastTo<T10>();
                m_Action(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
            }
        }
        public MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 11) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                var p4 = list[3].CastTo<T4>();
                var p5 = list[4].CastTo<T5>();
                var p6 = list[5].CastTo<T6>();
                var p7 = list[6].CastTo<T7>();
                var p8 = list[7].CastTo<T8>();
                var p9 = list[8].CastTo<T9>();
                var p10 = list[9].CastTo<T10>();
                var p11 = list[10].CastTo<T11>();
                m_Action(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
            }
        }
        public MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> m_Action;
    }
    internal class GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : AbstractDelegation
    {
        public override Delegate Action
        {
            get {
                return m_Action;
            }
        }
        public override void Execute(IList<BoxedValue> list)
        {
            if (null != m_Action && null != list && list.Count >= 12) {
                var p1 = list[0].CastTo<T1>();
                var p2 = list[1].CastTo<T2>();
                var p3 = list[2].CastTo<T3>();
                var p4 = list[3].CastTo<T4>();
                var p5 = list[4].CastTo<T5>();
                var p6 = list[5].CastTo<T6>();
                var p7 = list[6].CastTo<T7>();
                var p8 = list[7].CastTo<T8>();
                var p9 = list[8].CastTo<T9>();
                var p10 = list[9].CastTo<T10>();
                var p11 = list[10].CastTo<T11>();
                var p12 = list[11].CastTo<T12>();
                m_Action(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
            }
        }
        public MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> m_Action;
    }
    /// <summary>
    /// 消息机制
    /// </summary>
    public class PublishSubscribeSystemWithIntEvent
    {
        #region subscribe
        public object Subscribe(int ev_name, MyAction subscriber) { return AddSubscriber(ev_name, new GenericDelegation { m_Action = subscriber }); }
        public object Subscribe<T1>(int ev_name, MyAction<T1> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1> { m_Action = subscriber }); }
        public object Subscribe<T1, T2>(int ev_name, MyAction<T1, T2> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3>(int ev_name, MyAction<T1, T2, T3> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3, T4>(int ev_name, MyAction<T1, T2, T3, T4> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3, T4> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3, T4, T5>(int ev_name, MyAction<T1, T2, T3, T4, T5> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3, T4, T5> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3, T4, T5, T6>(int ev_name, MyAction<T1, T2, T3, T4, T5, T6> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3, T4, T5, T6> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3, T4, T5, T6, T7>(int ev_name, MyAction<T1, T2, T3, T4, T5, T6, T7> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3, T4, T5, T6, T7> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3, T4, T5, T6, T7, T8>(int ev_name, MyAction<T1, T2, T3, T4, T5, T6, T7, T8> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int ev_name, MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8, T9> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int ev_name, MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(int ev_name, MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> { m_Action = subscriber }); }
        public object Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(int ev_name, MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> subscriber) { return AddSubscriber(ev_name, new GenericDelegation<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> { m_Action = subscriber }); }
        #endregion

        #region Publish
        /// <summary>
        /// 处理异常
        /// </summary>
        private void HandleException(int ev_name, Exception ex)
        {
            if (null != ex.InnerException) {
                ex = ex.InnerException;
            }
            LogSystem.Error("PublishSubscribe.Publish({0}) exception:{1}\n{2}", ev_name, ex.Message, ex.StackTrace);
        }

        private void PrintErrorMessage(int ev_name)
        {
            //不存在监听者，不用输出log，因为很多监听者都不存在
            //LogSystem.Error("Publish {0} {1}, 不存在监听者", ev_name, group);
        }
        public void AddIgnoreEvent(int ev_name)
        {
            EventStatistics.AddIgnoreEvent(ev_name);
        }
        private void HandlePublishError(int ev_name)
        {
            string stackInfo = new StackTrace().ToString();
            string msg = "event: " + ev_name + " 循环嵌套!!!\n" + stackInfo;
            LogSystem.Error(msg);
        }
        /// <summary>
        /// 这个方法保留供cs2dsl来适应泛型版本，script实际并不会调用到这里，而是调用OriginPublish的list参数版本
        /// </summary>
        /// <param name="ev_name"></param>
        /// <param name="parameters"></param>
        public void Publish(int ev_name, params object[] parameters)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    OnPublishToScript(ev_name, parameters);
                }
                var bvl = boxedValueListPool.Alloc();
                foreach(var p in parameters) {
                    bvl.Add(BoxedValue.FromObject(p));
                }
                OriginPublish(ev_name, bvl);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }
        }

        public void OriginPublish(int ev_name, BoxedValueList parameters)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            ExecuteGenericDelegation(ev_name, temp_[i], parameters);
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else if (list.Count == 1) {
                        ExecuteGenericDelegation(ev_name, list[0], parameters);
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
            boxedValueListPool.Recycle(parameters);
        }

        /// <summary>
        /// 没有参数的通知:没有参数，不需要转化参数类型
        /// </summary>
        public void Publish(int ev_name)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name);
                }
                OriginPublish(ev_name);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish(int ev_name)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            MyAction tAct = temp_[i].Action as MyAction;
                            if (tAct != null)
                                tAct();
                            else
                                LogSystem.Error("Publish {0}, Action类型转换错误", ev_name);
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        MyAction tAct = list[0].Action as MyAction;
                        if (tAct != null)
                            tAct();
                        else
                            LogSystem.Error("Publish {0}, Action类型转换错误", ev_name);
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 一个参数通知事件
        /// </summary>
        public void Publish<T1>(int ev_name, T1 t1)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1);
                }
                OriginPublish(ev_name, t1);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1>(int ev_name, T1 t1)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1> tAct = tempAct.Action as MyAction<T1>;
                            if (tAct != null)
                                tAct(t1);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }

                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1> tAct = tempAct.Action as MyAction<T1>;
                        if (tAct != null)
                            tAct(t1);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 两个参数通知事件
        /// </summary>
        public void Publish<T1, T2>(int ev_name, T1 t1, T2 t2)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2);
                }
                OriginPublish(ev_name, t1, t2);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2>(int ev_name, T1 t1, T2 t2)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2> tAct = tempAct.Action as MyAction<T1, T2>;
                            if (tAct != null)
                                tAct(t1, t2);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2> tAct = tempAct.Action as MyAction<T1, T2>;
                        if (tAct != null)
                            tAct(t1, t2);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 三个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3>(int ev_name, T1 t1, T2 t2, T3 t3)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3);
                }
                OriginPublish(ev_name, t1, t2, t3);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2, T3>(int ev_name, T1 t1, T2 t2, T3 t3)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3> tAct = tempAct.Action as MyAction<T1, T2, T3>;
                            if (tAct != null)
                                tAct(t1, t2, t3);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3> tAct = tempAct.Action as MyAction<T1, T2, T3>;
                        if (tAct != null)
                            tAct(t1, t2, t3);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 四个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3, T4>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3, t4);
                }
                OriginPublish(ev_name, t1, t2, t3, t4);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }


        }
        public void OriginPublish<T1, T2, T3, T4>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3, T4> tAct = tempAct.Action as MyAction<T1, T2, T3, T4>;
                            if (tAct != null)
                                tAct(t1, t2, t3, t4);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                bvl.Add(BoxedValue.From(t4));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3, T4> tAct = tempAct.Action as MyAction<T1, T2, T3, T4>;
                        if (tAct != null)
                            tAct(t1, t2, t3, t4);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            bvl.Add(BoxedValue.From(t4));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 五个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3, T4, T5>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3, t4, t5);
                }
                OriginPublish(ev_name, t1, t2, t3, t4, t5);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2, T3, T4, T5>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3, T4, T5> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5>;
                            if (tAct != null)
                                tAct(t1, t2, t3, t4, t5);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                bvl.Add(BoxedValue.From(t4));
                                bvl.Add(BoxedValue.From(t5));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3, T4, T5> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5>;
                        if (tAct != null)
                            tAct(t1, t2, t3, t4, t5);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            bvl.Add(BoxedValue.From(t4));
                            bvl.Add(BoxedValue.From(t5));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 六个个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3, T4, T5, T6>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3, t4, t5, t6);
                }
                OriginPublish(ev_name, t1, t2, t3, t4, t5, t6);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2, T3, T4, T5, T6>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3, T4, T5, T6> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6>;
                            if (tAct != null)
                                tAct(t1, t2, t3, t4, t5, t6);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                bvl.Add(BoxedValue.From(t4));
                                bvl.Add(BoxedValue.From(t5));
                                bvl.Add(BoxedValue.From(t6));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3, T4, T5, T6> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6>;
                        if (tAct != null)
                            tAct(t1, t2, t3, t4, t5, t6);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            bvl.Add(BoxedValue.From(t4));
                            bvl.Add(BoxedValue.From(t5));
                            bvl.Add(BoxedValue.From(t6));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 七个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3, T4, T5, T6, T7>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3, t4, t5, t6, t7);
                }
                OriginPublish(ev_name, t1, t2, t3, t4, t5, t6, t7);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2, T3, T4, T5, T6, T7>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3, T4, T5, T6, T7> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7>;
                            if (tAct != null)
                                tAct(t1, t2, t3, t4, t5, t6, t7);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                bvl.Add(BoxedValue.From(t4));
                                bvl.Add(BoxedValue.From(t5));
                                bvl.Add(BoxedValue.From(t6));
                                bvl.Add(BoxedValue.From(t7));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3, T4, T5, T6, T7> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7>;
                        if (tAct != null)
                            tAct(t1, t2, t3, t4, t5, t6, t7);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            bvl.Add(BoxedValue.From(t4));
                            bvl.Add(BoxedValue.From(t5));
                            bvl.Add(BoxedValue.From(t6));
                            bvl.Add(BoxedValue.From(t7));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 八个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3, T4, T5, T6, T7, T8>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3, t4, t5, t6, t7, t8);
                }
                OriginPublish(ev_name, t1, t2, t3, t4, t5, t6, t7, t8);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2, T3, T4, T5, T6, T7, T8>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3, T4, T5, T6, T7, T8> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8>;
                            if (tAct != null)
                                tAct(t1, t2, t3, t4, t5, t6, t7, t8);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                bvl.Add(BoxedValue.From(t4));
                                bvl.Add(BoxedValue.From(t5));
                                bvl.Add(BoxedValue.From(t6));
                                bvl.Add(BoxedValue.From(t7));
                                bvl.Add(BoxedValue.From(t8));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3, T4, T5, T6, T7, T8> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8>;
                        if (tAct != null)
                            tAct(t1, t2, t3, t4, t5, t6, t7, t8);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            bvl.Add(BoxedValue.From(t4));
                            bvl.Add(BoxedValue.From(t5));
                            bvl.Add(BoxedValue.From(t6));
                            bvl.Add(BoxedValue.From(t7));
                            bvl.Add(BoxedValue.From(t8));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 九个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3, t4, t5, t6, t7, t8, t9);
                }
                OriginPublish(ev_name, t1, t2, t3, t4, t5, t6, t7, t8, t9);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>;
                            if (tAct != null)
                                tAct(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                bvl.Add(BoxedValue.From(t4));
                                bvl.Add(BoxedValue.From(t5));
                                bvl.Add(BoxedValue.From(t6));
                                bvl.Add(BoxedValue.From(t7));
                                bvl.Add(BoxedValue.From(t8));
                                bvl.Add(BoxedValue.From(t9));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>;
                        if (tAct != null)
                            tAct(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            bvl.Add(BoxedValue.From(t4));
                            bvl.Add(BoxedValue.From(t5));
                            bvl.Add(BoxedValue.From(t6));
                            bvl.Add(BoxedValue.From(t7));
                            bvl.Add(BoxedValue.From(t8));
                            bvl.Add(BoxedValue.From(t9));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 十个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                }
                OriginPublish(ev_name, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>;
                            if (tAct != null)
                                tAct(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                bvl.Add(BoxedValue.From(t4));
                                bvl.Add(BoxedValue.From(t5));
                                bvl.Add(BoxedValue.From(t6));
                                bvl.Add(BoxedValue.From(t7));
                                bvl.Add(BoxedValue.From(t8));
                                bvl.Add(BoxedValue.From(t9));
                                bvl.Add(BoxedValue.From(t10));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>;
                        if (tAct != null)
                            tAct(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            bvl.Add(BoxedValue.From(t4));
                            bvl.Add(BoxedValue.From(t5));
                            bvl.Add(BoxedValue.From(t6));
                            bvl.Add(BoxedValue.From(t7));
                            bvl.Add(BoxedValue.From(t8));
                            bvl.Add(BoxedValue.From(t9));
                            bvl.Add(BoxedValue.From(t10));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 十一个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                }
                OriginPublish(ev_name, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>;
                            if (tAct != null)
                                tAct(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                bvl.Add(BoxedValue.From(t4));
                                bvl.Add(BoxedValue.From(t5));
                                bvl.Add(BoxedValue.From(t6));
                                bvl.Add(BoxedValue.From(t7));
                                bvl.Add(BoxedValue.From(t8));
                                bvl.Add(BoxedValue.From(t9));
                                bvl.Add(BoxedValue.From(t10));
                                bvl.Add(BoxedValue.From(t11));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>;
                        if (tAct != null)
                            tAct(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            bvl.Add(BoxedValue.From(t4));
                            bvl.Add(BoxedValue.From(t5));
                            bvl.Add(BoxedValue.From(t6));
                            bvl.Add(BoxedValue.From(t7));
                            bvl.Add(BoxedValue.From(t8));
                            bvl.Add(BoxedValue.From(t9));
                            bvl.Add(BoxedValue.From(t10));
                            bvl.Add(BoxedValue.From(t11));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }

        /// <summary>
        /// 十二个参数通知事件
        /// </summary>
        public void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
        {
            try {
                if (EventStatistics.IsEventPublished(ev_name)) {
                    HandlePublishError(ev_name);
                    return;
                }
                EventStatistics.SetEventPublished(ev_name, true);
                if (IsExistScriptListener(ev_name) && null != OnPublishToScript) {
                    PublishToScript(ev_name, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                }
                OriginPublish(ev_name, t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            }
            catch (Exception e) {

            }
            finally {
                EventStatistics.SetEventPublished(ev_name, false);
            }

        }
        public void OriginPublish<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(int ev_name, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
        {
            try {
                List<IDelegation> list;
                if (subscribers_.TryGetValue(ev_name, out list) && list != null) {
                    if (list.Count > 1) {
                        //拷贝到临时列表，防止重入操作修改list                            
                        var temp_ = tempListPool.Alloc();// new List<IDelegation>();
                        temp_.AddRange(list);
                        for (int i = 0; i < temp_.Count; ++i) {
                            IDelegation tempAct = temp_[i];
                            MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>;
                            if (tAct != null)
                                tAct(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                            else {
                                var bvl = boxedValueListPool.Alloc();
                                bvl.Add(BoxedValue.From(t1));
                                bvl.Add(BoxedValue.From(t2));
                                bvl.Add(BoxedValue.From(t3));
                                bvl.Add(BoxedValue.From(t4));
                                bvl.Add(BoxedValue.From(t5));
                                bvl.Add(BoxedValue.From(t6));
                                bvl.Add(BoxedValue.From(t7));
                                bvl.Add(BoxedValue.From(t8));
                                bvl.Add(BoxedValue.From(t9));
                                bvl.Add(BoxedValue.From(t10));
                                bvl.Add(BoxedValue.From(t11));
                                bvl.Add(BoxedValue.From(t12));
                                ExecuteGenericDelegation(ev_name, tempAct, bvl);
                                boxedValueListPool.Recycle(bvl);
                            }
                        }
                        tempListPool.Recycle(temp_);
                    }
                    else {
                        IDelegation tempAct = list[0];
                        MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> tAct = tempAct.Action as MyAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>;
                        if (tAct != null)
                            tAct(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                        else {
                            var bvl = boxedValueListPool.Alloc();
                            bvl.Add(BoxedValue.From(t1));
                            bvl.Add(BoxedValue.From(t2));
                            bvl.Add(BoxedValue.From(t3));
                            bvl.Add(BoxedValue.From(t4));
                            bvl.Add(BoxedValue.From(t5));
                            bvl.Add(BoxedValue.From(t6));
                            bvl.Add(BoxedValue.From(t7));
                            bvl.Add(BoxedValue.From(t8));
                            bvl.Add(BoxedValue.From(t9));
                            bvl.Add(BoxedValue.From(t10));
                            bvl.Add(BoxedValue.From(t11));
                            bvl.Add(BoxedValue.From(t12));
                            ExecuteGenericDelegation(ev_name, tempAct, bvl);
                            boxedValueListPool.Recycle(bvl);
                        }
                    }
                }
                else {
                    PrintErrorMessage(ev_name);
                }
            }
            catch (Exception ex) {
                HandleException(ev_name, ex);
            }
        }
        #endregion

        #region 添加跟移除
        /// <summary>
        /// objDelegate 这个参数是进行了object类型转换的。
        /// </summary>
        private object AddSubscriber(int ev_name, IDelegation d)
        {
            SubscribeKeyToScript(ev_name);
            List<IDelegation> source;
            if (subscribers_.TryGetValue(ev_name, out source)) {
                //查找有没有重的
                for (int i = 0; i < source.Count; i++) {
                    if (source[i] == d) {
                        return new ReceiptInfo(ev_name, null);
                    }
                }
                source.Add(d);
            }
            else {
                source = new List<IDelegation>();
                source.Add(d);
                subscribers_.Add(ev_name, source);
            }
            return new ReceiptInfo(ev_name, d);
        }

        public void removeAllEventName(int ev_name)
        {
            UnSubscribeKeyToScript(ev_name);
            List<IDelegation> list;
            if (subscribers_.TryGetValue(ev_name, out list)) {
                if (list != null) {
                    list.Clear();
                    subscribers_.Remove(ev_name);
                }
            }
        }

        public void Unsubscribe(object receipt)
        {
            if (receipt == null) return;
            ReceiptInfo r = receipt as ReceiptInfo;
            if (null != r && r.delegate_ != null) {
                UnSubscribeKeyToScript(r.name_);
                List<IDelegation> list;
                if (null != r) {
                    if (subscribers_.TryGetValue(r.name_, out list)) {
                        if (list != null) {
                            list.Remove(r.delegate_);
                            if (list.Count == 0) {
                                subscribers_.Remove(r.name_);
                            }
                        }
                    }
                }
            }
        }

        public void LogCount(string tag)
        {
            LogSystem.GmLog("{0} PublishSubscribeSystemBase Subscribed Event Count {1}", tag, subscribers_.Count);
            foreach (var pair in subscribers_) {
                LogSystem.GmLog("{0} {1} Subscriber Count {2}", tag, pair.Key, pair.Value.Count);
            }
            LogSystem.GmLog("{0} PublishSubscribeSystemBase LusListenerDict Count {1}", tag, m_ScriptListenerDic.Count);
            LogSystem.GmLog("{0} PublishSubscribeSystemBase ToTellLuaDic Count {1}", tag, m_toTellScriptDic.Count);
        }
        #endregion

        #region 通过IDelegation.Execute接口来调用Action
        private void ExecuteGenericDelegation(int ev_name, IDelegation action, IList<BoxedValue> parameters)
        {
            if (action == null) {
                return;
            }
            if (parameters == null || parameters.Count == 0) {
                action.Execute(parameters);
                return;
            }
            //参数个数必须是匹配的
            var paraInfos = action.Parameters;
            if (null != paraInfos) {
                for (int i = 0; i < paraInfos.Length; ++i) {
                    var paraInfo = paraInfos[i];
                    if (i < parameters.Count) {
                        var p = parameters[i];
                        if (p.IsString && paraInfo.ParameterType != typeof(string) || !p.IsString && paraInfo.ParameterType == typeof(string)) {
                            LogSystem.Error("event {0} invoke with dismatch argument: {1} type {2} from BoxedValue type {3} ", ev_name, i, paraInfo.ParameterType.Name, p.GetTypeName());
                            Helper.LogCallStack(true);
                        }
                    }
                    else {
                        LogSystem.Error("event {0} invoke without argument: {1} type {2}", ev_name, i, paraInfo.ParameterType.Name);
                        Helper.LogCallStack(true);
                    }
                }
            }
            action.Execute(parameters);
        }
        #endregion

        #region script那边的监听事件的管理
        /// <summary>
        /// script 那边的监听者
        /// </summary>
        protected static Dictionary<int, int> m_ScriptListenerDic = new Dictionary<int, int>();
        public delegate void PublishToScriptDelegation(int ev_name, object[] parameters);
        public PublishToScriptDelegation OnPublishToScript;

        public delegate void EventKeyToScriptDelegate(int ev_name);
        public EventKeyToScriptDelegate SubscribeKeyToScriptDelegate;
        public EventKeyToScriptDelegate UnsubscribeKeyToScriptDelegate;

        private Dictionary<int, int> m_toTellScriptDic = new Dictionary<int, int>();
        /// <summary>
        /// script 那边添加监听
        /// </summary>
        public void ScriptSubscribeEvent(int ev_name)
        {
            int refcnt = 0;
            if (m_ScriptListenerDic.ContainsKey(ev_name)) {
                refcnt = m_ScriptListenerDic[ev_name];
            }
            refcnt = refcnt + 1;
            m_ScriptListenerDic[ev_name] = refcnt;
            LogSystem.Warn("-------script add key {0} ", ev_name);
        }

        /// <summary>
        /// script 那边移除监听
        /// </summary>
        public void ScriptUnSubscribeEvent(int ev_name)
        {
            LogSystem.Warn("-------script remove key {0}", ev_name);
            int refcnt = 0;
            if (m_ScriptListenerDic.TryGetValue(ev_name, out refcnt)) {
                refcnt = refcnt - 1;
                if (refcnt <= 0) {
                    m_ScriptListenerDic.Remove(ev_name);
                }
                else {
                    m_ScriptListenerDic[ev_name] = refcnt;
                }
            }
        }

        private Array cs2dslEventTypeArr = null;
        public static Type cs2dslEventType = null;
        private const bool DebugPrintLog = false;

        private void CheckPrintEventLog(int ev_name)
        {
            if (!DebugPrintLog) {
                return;
            }
            try {
                if (cs2dslEventTypeArr == null) {
                    if (cs2dslEventType != null) {
                        cs2dslEventTypeArr = cs2dslEventType.GetEnumValues();
                    }
                }
                if (cs2dslEventTypeArr != null) {
                    foreach (var item in cs2dslEventTypeArr) {
                        if ((int)item == ev_name) {
                            LogSystem.Warn("-------GameEventLua事件-----------  " + item.ToString());
                            break;
                        }
                    }
                }
                else {
                    LogSystem.Warn("------GameEventLib事件------------  " + ev_name);
                }
            }
            catch (Exception e) {
                LogSystem.Warn("----!!!!-------------   " + e.ToString() + "        " + ev_name);
            }
        }

        /// <summary>
        /// 检测是不是存在 script监听者
        /// </summary>
        protected bool IsExistScriptListener(int ev_name)
        {
            CheckPrintEventLog(ev_name);
            int refcnt = 0;
            if (m_ScriptListenerDic.TryGetValue(ev_name, out refcnt)) {
                return refcnt > 0;
            }
            return false;
        }

        /// <summary>
        /// 如果script ，这边订阅了，发布给script
        /// </summary>
        protected void PublishToScript(int ev_name, params object[] parameters)
        {
            //如果script 那边有监听者,发布事件给script
            OnPublishToScript(ev_name, parameters);
        }

        /// <summary>
        /// 添加监听事件到 script 那边
        /// </summary>
        private void SubscribeKeyToScript(int ev_name)
        {
            if (SubscribeKeyToScriptDelegate != null) {
                SubscribeKeyToScriptDelegate(ev_name);
                if (m_toTellScriptDic.Count > 0) {
                    foreach (var k in m_toTellScriptDic) {
                        for (int i = 0; i < k.Value; i++) {
                            SubscribeKeyToScriptDelegate(k.Key);
                        }
                    }
                    m_toTellScriptDic.Clear();
                }
            }
            else {
                int exist = 0;
                if (m_toTellScriptDic.TryGetValue(ev_name, out exist)) {
                    m_toTellScriptDic[ev_name] = exist + 1;
                }
                else {
                    m_toTellScriptDic[ev_name] = 1;
                }
            }
        }

        /// <summary>
        /// 移除key ，从script 那边
        /// </summary>
        private void UnSubscribeKeyToScript(int ev_name)
        {
            if (UnsubscribeKeyToScriptDelegate != null) {
                UnsubscribeKeyToScriptDelegate(ev_name);
            }
            else {
                int exist = 0;
                if (m_toTellScriptDic.TryGetValue(ev_name, out exist)) {
                    if (exist > 1) {
                        m_toTellScriptDic[ev_name] = exist - 1;
                    }
                    else {
                        m_toTellScriptDic.Remove(ev_name);
                    }
                }
            }
        }
        #endregion

        private class ReceiptInfo
        {
            public short name_;
            public IDelegation delegate_;
            public ReceiptInfo() { }
            public ReceiptInfo(int n, IDelegation d)
            {
                name_ = (short)n;
                delegate_ = d;
            }
        }

        /// <summary>
        /// 存储所有的监听者
        /// </summary>
        private Dictionary<int, List<IDelegation>> subscribers_ = new Dictionary<int, List<IDelegation>>();

        private static SimpleObjectListPool<IDelegation> tempListPool = new SimpleObjectListPool<IDelegation>(2, 4);
        private static SimpleObjectPool<BoxedValueList> boxedValueListPool = new SimpleObjectPool<BoxedValueList>(1024);
    }
}
