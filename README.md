# cs583_3DGroupProject
3D tower defense game for cs583
Game Title: KingdomFell
Group Members: Michael D, Nathan F, Nicholas S, Clinton C

Current Direction of Game: A 3D tower defense game with 3-4 varying maps that will have some different mechanics across each map (currently testing with a test map to get the basics down, will most likely be kept as the tutorial). Forest Map (easiest of the three with the standard line of enemy evolutions, such as basic skeletons that have different variance that will indicate their amount of hp, fast but frail ones, tanky but slow ones, magic resist ones, physical resist ones, invis/camo ones, potentially etc), Winter Map (includes all the forest map ones, freeze/stun/slow mechanic inflicted on turrets (ie. reduce atk speed or maybe reduce turret range to 0 to stimulate stun/freeze for a set duration), potentially destroy turrets/game objects), River Map (same as forest mobs, flying enemies that can only be targeted by certain turrets (ie. archer and ballistic tower), projectile destroying ones, potentially etc).
Current turrets (from assets/prefabs) include archer tower, ballista tower, cannon tower, poison tower, and wizard tower.
Archer tower - Will target air and ground, has somewhat long range. At further upgrade stages, increase the amount of targets that can be hit, faster attack speed (already decent starting attack speed), longer range, one stage of damage increase (or 2) at later stages of upgrade, bonus damage to magic resistance units at later stages, able to see invis/camo at later stages.
Canncon tower - Most basic, well-rounded stats, low-medium range, moderate attack speed, basic damage. For upgrades, slightly increase range at some stages maybe, increase damage, add aoe at some stage of upgrade.
Ballista tower - Sniepr class tower, long range, high damage, slow attack rate. Upgrades might include pierce, even more damage, slightly increase attack rate, more range, physical resistance pierce (armor pen) at later stages of upgrade, able to see camo/invis at later stages.
Poison tower - Damage over time per hit, moderate range, cc abilities, magic damage. At upgrades, maybe add slow, longer dot effect, could maybe leave a lingering field for a set time (deltatime) that will deal dmg to any mob in it, ignore magic resistance at later stages of upgrade, remove physical resistance briefly. can damage camo/invis through lingering field but cannot actively target them.
Wizard tower - Targets ground with a chance to target air at later stages, moderate range and magic damage. Upgrades might include chaining targets with a possibility to hit air through the chain, range increase slightly with each upgrade, damage increase at some stages, increased chain.

General Goals:
Michael Dinh - Programmer, level design, asset implementation
Nathan F - Asset creation, animation
Nicholas S - Asset creation
Clinton D - Programmer, level design
