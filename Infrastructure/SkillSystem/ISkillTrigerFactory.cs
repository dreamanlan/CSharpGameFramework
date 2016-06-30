using System;
using System.Collections.Generic;

namespace SkillSystem
{
    public interface ISkillTrigerFactory
    {
        ISkillTriger Create();
    }
    public class SkillTrigerFactoryHelper<T> : ISkillTrigerFactory where T : ISkillTriger, new()
    {
        public ISkillTriger Create()
        {
            T t = new T();
            return t;
        }
    }
}
