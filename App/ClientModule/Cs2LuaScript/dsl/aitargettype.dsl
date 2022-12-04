require("cs2dsl__syslib");
require("cs2dsl__namespaces");
require("cs2dsl__externenums");
require("cs2dsl__interfaces");

enum(AiTargetType, System.Enum, true) {
	member("NPC", 0);
	member("HERO", 1);
	member("BOSS", 2);
	member("ALL", 3);
};



