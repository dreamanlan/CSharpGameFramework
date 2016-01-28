tabledef(Actor, dictionary, client)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(name, name, string);
	fielddef(icon, icon, int32);
	fielddef(type, type, int32);
	fielddef(avatar, avatar, string);
	fielddef(cooldown, cooldown, float);
	fielddef(hp, hp, int32);
	fielddef(mp, mp, int32);
	fielddef(baseattack, baseattack, int32);
	fielddef(defence, defence, int32);
	fielddef(speed, speed, float);
	fielddef(viewrange, viewrange, float);
	fielddef(gohomerange, gohomerange, float);
	fielddef(skill0, skill0, int32);
	fielddef(skill1, skill1, int32);
	fielddef(skill2, skill2, int32);
	fielddef(skill3, skill3, int32);
	fielddef(skill4, skill4, int32);
	fielddef(passiveskill1, passiveskill1, int32);
	fielddef(passiveskill2, passiveskill2, int32);
	fielddef(passiveskill3, passiveskill3, int32);
	fielddef(passiveskill4, passiveskill4, int32);
	fielddef(bornskill, bornskill, int32);
	fielddef(deadskill, deadskill, int32);
	fielddef(size, size, float);
};
tabledef(Formation, dictionary, client)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(teamid, teamid, int32);
	fielddef(pos0, pos0, float_list);
	fielddef(dir0, dir0, float);
	fielddef(pos1, pos1, float_list);
	fielddef(dir1, dir1, float);
	fielddef(pos2, pos2, float_list);
	fielddef(dir2, dir2, float);
	fielddef(pos3, pos3, float_list);
	fielddef(dir3, dir3, float);
	fielddef(pos4, pos4, float_list);
	fielddef(dir4, dir4, float);
	fielddef(pos5, pos5, float_list);
	fielddef(dir5, dir5, float);
	fielddef(pos6, pos6, float_list);
	fielddef(dir6, dir6, float);
	fielddef(pos7, pos7, float_list);
	fielddef(dir7, dir7, float);
	fielddef(pos8, pos8, float_list);
	fielddef(dir8, dir8, float);
	fielddef(pos9, pos9, float_list);
	fielddef(dir9, dir9, float);
	fielddef(pos10, pos10, float_list);
	fielddef(dir10, dir10, float);
	fielddef(pos11, pos11, float_list);
	fielddef(dir11, dir11, float);
	fielddef(pos12, pos12, float_list);
	fielddef(dir12, dir12, float);
	fielddef(pos13, pos13, float_list);
	fielddef(dir13, dir13, float);
	fielddef(pos14, pos14, float_list);
	fielddef(dir14, dir14, float);
	fielddef(pos15, pos15, float_list);
	fielddef(dir15, dir15, float);
};
tabledef(Level, dictionary, client)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(prefab, prefab, string);
	fielddef(type, type, int32);
	fielddef(GfxDslFile, GfxDslFile, string_list);
	fielddef(SceneUi, SceneUi, int32_list);
};
tabledef(Skill, dictionary, client)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(desc, desc, string);
	fielddef(type, type, int32);
	fielddef(icon, icon, int32);
	fielddef(distance, distance, float);
	fielddef(cooldown, cooldown, float);
	fielddef(duration, duration, float);
	fielddef(interval, interval, float);
	fielddef(damage, damage, int32);
	fielddef(mpRecover, mpRecover, int32);
	fielddef(hpRecover, hpRecover, int32);
	fielddef(addAttack, addAttack, int32);
	fielddef(addDefence, addDefence, int32);
	fielddef(addRps, addRps, int32);
	fielddef(addCritical, addCritical, int32);
	fielddef(addCriticalPow, addCriticalPow, int32);
	fielddef(addSpeed, addSpeed, float);
	fielddef(addShield, addShield, int32);
	fielddef(canmove, canmove, int32);
	fielddef(impactToSelf, impactToSelf, int32);
	fielddef(impactToTarget, impactToTarget, int32);
	fielddef(summonActor, summonActor, int32);
	fielddef(interruptPriority, interruptPriority, int32);
	fielddef(isInterrupt, isInterrupt, bool);
	fielddef(targetType, targetType, int32);
	fielddef(aoeType, aoeType, int32);
	fielddef(aoeSize, aoeSize, float);
	fielddef(aoeAngleOrLength, aoeAngleOrLength, float);
	fielddef(maxAoeTargetCount, maxAoeTargetCount, int32);
	fielddef(dslSkillId, dslSkillId, int32);
};
tabledef(SkillDsl, dictionary, client)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(dslFile, dslFile, string);
};
tabledef(SkillResources, list, client)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(skillId, skillId, int32);
	fielddef(key, key, string);
	fielddef(resource, resource, string);
};
tabledef(StoryDlg, dictionary, client)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(dialogId, dialogId, int32);
	fielddef(index, index, int32);
	fielddef(speaker, speaker, int32);
	fielddef(leftOrRight, leftOrRight, int32);
	fielddef(dialog, dialog, string);
};
tabledef(StrDictionary, dictionary, client)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, string);
	fielddef(Content, Content, string);
};
tabledef(UI, dictionary, client)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(name, name, string);
	fielddef(path, path, string);
	fielddef(dsl, dsl, string);
	fielddef(visible, visible, bool);
};
