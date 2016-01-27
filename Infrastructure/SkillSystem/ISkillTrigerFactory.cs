using System;
using System.Collections.Generic;

namespace SkillSystem
{
    public interface ISkillTrigerFactory
    {
        ISkillTriger Create(Dsl.ISyntaxComponent trigerConfig, int dslSkillId);
    }
    public class SkillTrigerFactoryHelper<T> : ISkillTrigerFactory where T : ISkillTriger, new()
    {
        public ISkillTriger Create(Dsl.ISyntaxComponent trigerConfig, int dslSkillId)
        {
            T t = new T();
            t.Init(trigerConfig, dslSkillId);
            return t;
        }
    }
}
