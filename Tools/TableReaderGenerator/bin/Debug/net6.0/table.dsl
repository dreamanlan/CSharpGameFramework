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
tabledef(Actor, dictionary, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(name, name, string);
	fielddef(icon, icon, int32);
	fielddef(bigIcon, bigIcon, int32);
	fielddef(type, type, int32);
	fielddef(avatar, avatar, string);
	fielddef(skill0, skill0, int32);
	fielddef(skill1, skill1, int32);
	fielddef(skill2, skill2, int32);
	fielddef(skill3, skill3, int32);
	fielddef(skill4, skill4, int32);
	fielddef(skill5, skill5, int32);
	fielddef(skill6, skill6, int32);
	fielddef(skill7, skill7, int32);
	fielddef(skill8, skill8, int32);
	fielddef(bornskill, bornskill, int32);
	fielddef(deadskill, deadskill, int32);
	fielddef(size, size, float);
	fielddef(logicsize, logicsize, float);
	fielddef(speed, speed, float);
	fielddef(viewrange, viewrange, float);
	fielddef(gohomerange, gohomerange, float);
	fielddef(x4001, x4001, int32);
	fielddef(x4002, x4002, int32);
	fielddef(x4003, x4003, int32);
	fielddef(x4004, x4004, int32);
	fielddef(x1001, x1001, int32);
	fielddef(x1002, x1002, int32);
	fielddef(x1006, x1006, int32);
	fielddef(x1007, x1007, int32);
	fielddef(x1011, x1011, int32);
	fielddef(x1012, x1012, int32);
	fielddef(x1016, x1016, int32);
	fielddef(x1017, x1017, int32);
	fielddef(x1021, x1021, int32);
	fielddef(x1022, x1022, int32);
	fielddef(x1024, x1024, int32);
	fielddef(x1026, x1026, int32);
	fielddef(x1028, x1028, int32);
	fielddef(x1030, x1030, int32);
	fielddef(x1032, x1032, int32);
	fielddef(x1033, x1033, int32);
	fielddef(x1034, x1034, int32);
	fielddef(x2001, x2001, int32);
	fielddef(x2002, x2002, int32);
	fielddef(x2007, x2007, int32);
	fielddef(x2008, x2008, int32);
};
tabledef(AttrDefine, dictionary, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(value, value, int32);
	fielddef(minValue, minValue, int32);
	fielddef(maxValue, maxValue, int32);
};
tabledef(Const, dictionary, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(value, value, int32);
};
tabledef(Formation, dictionary, public)
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
tabledef(ImpactData, dictionary, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(desc, desc, string);
	fielddef(type, type, int32);
	fielddef(icon, icon, int32);
	fielddef(duration, duration, int32);
	fielddef(cooldown, cooldown, int32);
	fielddef(multiple, multiple, int32_list);
	fielddef(damage, damage, int32_list);
	fielddef(vampire, vampire, int32_list);
	fielddef(attr1, attr1, int32);
	fielddef(value1, value1, int32);
	fielddef(attr2, attr2, int32);
	fielddef(value2, value2, int32);
	fielddef(attr3, attr3, int32);
	fielddef(value3, value3, int32);
	fielddef(attr4, attr4, int32);
	fielddef(value4, value4, int32);
	fielddef(attr5, attr5, int32);
	fielddef(value5, value5, int32);
	fielddef(attr6, attr6, int32);
	fielddef(value6, value6, int32);
	fielddef(attr7, attr7, int32);
	fielddef(value7, value7, int32);
	fielddef(attr8, attr8, int32);
	fielddef(value8, value8, int32);
};
tabledef(Level, dictionary, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(prefab, prefab, string);
	fielddef(type, type, int32);
	fielddef(SceneDslFile, SceneDslFile, string_list);
	fielddef(ClientDslFile, ClientDslFile, string_list);
	fielddef(RoomDslFile, RoomDslFile, string_list);
	fielddef(SceneUi, SceneUi, int32_list);
	fielddef(EnterX, EnterX, float);
	fielddef(EnterY, EnterY, float);
	fielddef(EnterRadius, EnterRadius, float);
	fielddef(RoomServer, RoomServer, string_list);
	fielddef(ThreadCountPerScene, ThreadCountPerScene, int32);
	fielddef(RoomCountPerThread, RoomCountPerThread, int32);
	fielddef(MaxUserCount, MaxUserCount, int32);
	fielddef(CanPK, CanPK, bool);
};
tabledef(LevelMonster, list, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(group, group, int32);
	fielddef(scene, scene, int32);
	fielddef(camp, camp, int32);
	fielddef(actorID, actorID, int32);
	fielddef(x, x, float);
	fielddef(y, y, float);
	fielddef(dir, dir, float);
	fielddef(level, level, int32);
	fielddef(passive, passive, bool);
	fielddef(aiLogic, aiLogic, string);
	fielddef(aiParams, aiParams, string_list);
};
tabledef(Skill, dictionary, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(desc, desc, string);
	fielddef(type, type, int32);
	fielddef(icon, icon, int32);
	fielddef(impacttoself, impacttoself, int32);
	fielddef(impact, impact, int32);
	fielddef(targetType, targetType, int32);
	fielddef(aoeType, aoeType, int32);
	fielddef(aoeSize, aoeSize, float);
	fielddef(aoeAngleOrLength, aoeAngleOrLength, float);
	fielddef(maxAoeTargetCount, maxAoeTargetCount, int32);
	fielddef(dslFile, dslFile, string);
};
tabledef(SkillData, dictionary, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, int32);
	fielddef(desc, desc, string);
	fielddef(type, type, int32);
	fielddef(icon, icon, int32);
	fielddef(distance, distance, float);
	fielddef(cooldown, cooldown, int32);
	fielddef(canmove, canmove, int32);
	fielddef(interruptPriority, interruptPriority, int32);
	fielddef(isInterrupt, isInterrupt, bool);
	fielddef(subsequentSkills, subsequentSkills, int32_list);
	fielddef(autoCast, autoCast, int32);
	fielddef(needTarget, needTarget, bool);
	fielddef(multiple, multiple, int32_list);
	fielddef(damage, damage, int32_list);
	fielddef(vampire, vampire, int32_list);
	fielddef(addsc, addsc, int32);
	fielddef(beaddsc, beaddsc, int32);
	fielddef(adduc, adduc, int32);
	fielddef(beadduc, beadduc, int32);
	fielddef(attr1, attr1, int32);
	fielddef(value1, value1, int32);
	fielddef(attr2, attr2, int32);
	fielddef(value2, value2, int32);
	fielddef(attr3, attr3, int32);
	fielddef(value3, value3, int32);
	fielddef(attr4, attr4, int32);
	fielddef(value4, value4, int32);
	fielddef(attr5, attr5, int32);
	fielddef(value5, value5, int32);
	fielddef(attr6, attr6, int32);
	fielddef(value6, value6, int32);
	fielddef(attr7, attr7, int32);
	fielddef(value7, value7, int32);
	fielddef(attr8, attr8, int32);
	fielddef(value8, value8, int32);
};
tabledef(SkillEvent, list, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(actorId, actorId, int32);
	fielddef(skillId, skillId, int32);
	fielddef(eventId, eventId, int32);
	fielddef(triggerBuffId, triggerBuffId, int32);
	fielddef(triggerSkillId, triggerSkillId, int32);
	fielddef(proc, proc, string);
	fielddef(desc, desc, string);
	fielddef(param1, param1, int32);
	fielddef(desc1, desc1, string);
	fielddef(param2, param2, int32);
	fielddef(desc2, desc2, string);
	fielddef(param3, param3, int32);
	fielddef(desc3, desc3, string);
	fielddef(param4, param4, int32);
	fielddef(desc4, desc4, string);
	fielddef(param5, param5, int32);
	fielddef(desc5, desc5, string);
	fielddef(param6, param6, int32);
	fielddef(desc6, desc6, string);
	fielddef(param7, param7, int32);
	fielddef(desc7, desc7, string);
	fielddef(param8, param8, int32);
	fielddef(desc8, desc8, string);
};
tabledef(SkillResources, list, public)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(skillId, skillId, int32);
	fielddef(key, key, string);
	fielddef(resource, resource, string);
};
tabledef(UserScript, dictionary, server)
{
	recordmodifier(partial);
	providermodifier(partial);
	fielddef(id, id, string);
	fielddef(StoryId, StoryId, string);
	fielddef(Namespace, Namespace, string);
	fielddef(DslFile, DslFile, string);
};
