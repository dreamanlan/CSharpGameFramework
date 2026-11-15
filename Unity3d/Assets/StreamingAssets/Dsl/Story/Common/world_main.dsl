story(world_main)
{
  // World-map grid (2087 modern layer). This layer has movement and mutual
  // visibility only: no combat, no server-side battle simulation. Client
  // move intents are handled by Msg_CR_UserMoveToPosHandler and broadcast
  // with Msg_RC_NpcMove to everyone in the field room.
  onmessage("start")
  {
  };
  onmessage("user_enter_scene")params($objId,$unitId,$campId,$x,$z)
  {
    log("world grid enter obj={0} unit={1} pos=({2},{3})", $objId, $unitId, $x, $z);
  };
};
