using System;
using System.Collections.Generic;

namespace DotnetSkillScript
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
