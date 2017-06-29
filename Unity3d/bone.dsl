object("monster")
{
	transform
	{
		position(0.000, 0.000, 0.000);
		rotation(0.000, 0.000, 0.000);
		scale(1.000, 1.000, 1.000);
	};
	component("Animator", "UnityEngine.Animator, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
	{
		controller("Assets/ThirdParty/FantasyMonster/Skeleton/Ani/Skeleton.controller");
		avatar("Assets/ThirdParty/FantasyMonster/Skeleton/Character/Skeleton@Skin.FBX");
		options(False, "Normal", "BasedOnRenderers");
	};
	component("CapsuleCollider", "UnityEngine.CapsuleCollider, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
	object("Bip001")
	{
		transform
		{
			position(0.000, 1.998, 0.000);
			rotation(270.000, 90.000, 0.000);
			scale(1.000, 1.000, 1.000);
		};
		object("Bip001 Footsteps")
		{
			transform
			{
				position(0.000, 0.000, -1.997);
				rotation(0.000, 0.000, 270.000);
				scale(1.000, 1.000, 1.000);
			};
		};
		object("Bip001 Pelvis")
		{
			transform
			{
				position(0.000, 0.000, 0.000);
				rotation(270.000, 90.000, 0.000);
				scale(1.000, 1.000, 1.000);
			};
			object("Bip001 Spine")
			{
				transform
				{
					position(-0.188, 0.095, 0.000);
					rotation(0.000, 0.000, 359.954);
					scale(1.000, 1.000, 1.000);
				};
				object("Bip001 L Thigh")
				{
					transform
					{
						position(0.188, -0.095, 0.346);
						rotation(353.128, 169.834, 354.414);
						scale(1.000, 1.000, 1.000);
					};
					object("Bip001 L Calf")
					{
						transform
						{
							position(-0.797, 0.000, 0.000);
							rotation(0.000, 0.000, 11.459);
							scale(1.000, 1.000, 1.000);
						};
						object("Bip001 L Foot")
						{
							transform
							{
								position(-0.995, 0.000, 0.000);
								rotation(348.980, 11.032, 355.297);
								scale(1.000, 1.000, 1.000);
							};
							object("Bip001 L Toe0")
							{
								transform
								{
									position(-0.242, 0.398, 0.000);
									rotation(0.000, 0.000, 270.000);
									scale(1.000, 1.000, 1.000);
								};
							};
						};
					};
				};
				object("Bip001 R Thigh")
				{
					transform
					{
						position(0.188, -0.095, -0.346);
						rotation(6.872, 190.166, 354.413);
						scale(1.000, 1.000, 1.000);
					};
					object("Bip001 R Calf")
					{
						transform
						{
							position(-0.797, 0.000, 0.000);
							rotation(0.000, 0.000, 11.459);
							scale(1.000, 1.000, 1.000);
						};
						object("Bip001 R Foot")
						{
							transform
							{
								position(-0.995, 0.000, 0.000);
								rotation(11.021, 348.968, 355.297);
								scale(1.000, 1.000, 1.000);
							};
							object("Bip001 R Toe0")
							{
								transform
								{
									position(-0.242, 0.398, 0.000);
									rotation(0.000, 0.000, 270.000);
									scale(1.000, 1.000, 1.000);
								};
							};
						};
					};
				};
				object("Bip001 Spine1")
				{
					transform
					{
						position(-0.539, -0.001, 0.000);
						rotation(0.000, 0.000, 0.000);
						scale(1.000, 1.000, 1.000);
					};
					object("Bip001 Neck")
					{
						transform
						{
							position(-0.899, -0.070, 0.000);
							rotation(0.000, 0.000, 342.119);
							scale(1.000, 1.000, 1.000);
						};
						object("Bip001 Head")
						{
							transform
							{
								position(-0.331, 0.000, 0.000);
								rotation(0.000, 0.000, 3.767);
								scale(1.000, 1.000, 1.000);
							};
							object("Bone001")
							{
								transform
								{
									position(-0.058, 0.006, 0.000);
									rotation(0.000, 180.000, 284.707);
									scale(1.000, 1.000, 1.000);
								};
							};
						};
						object("Bip001 L Clavicle")
						{
							transform
							{
								position(0.031, -0.029, 0.274);
								rotation(15.527, 300.815, 189.072);
								scale(1.000, 1.000, 1.000);
							};
							object("Bip001 L UpperArm")
							{
								transform
								{
									position(-0.296, 0.000, 0.000);
									rotation(4.524, 350.698, 352.846);
									scale(1.000, 1.000, 1.000);
								};
								object("Bip001 L Forearm")
								{
									transform
									{
										position(-0.713, 0.000, 0.000);
										rotation(0.000, 0.000, 12.381);
										scale(1.000, 1.000, 1.000);
									};
									object("Bip001 L Hand")
									{
										transform
										{
											position(-0.554, 0.000, 0.000);
											rotation(280.832, 160.662, 190.741);
											scale(1.000, 1.000, 1.000);
										};
										object("Bip001 L Finger0")
										{
											transform
											{
												position(-0.282, 0.025, -0.174);
												rotation(48.146, 280.778, 327.445);
												scale(1.000, 1.000, 1.000);
											};
											object("Bip001 L Finger01")
											{
												transform
												{
													position(-0.155, 0.000, 0.000);
													rotation(0.000, 0.000, 359.064);
													scale(1.000, 1.000, 1.000);
												};
												object("Bip001 L Finger02")
												{
													transform
													{
														position(-0.091, 0.000, 0.000);
														rotation(0.000, 0.000, 340.918);
														scale(1.000, 1.000, 1.000);
													};
												};
											};
										};
										object("Bip001 L Finger1")
										{
											transform
											{
												position(-0.531, 0.000, -0.012);
												rotation(0.496, 5.415, 348.463);
												scale(1.000, 1.000, 1.000);
											};
											object("Bip001 L Finger11")
											{
												transform
												{
													position(-0.102, 0.000, 0.000);
													rotation(0.000, 0.000, 340.529);
													scale(1.000, 1.000, 1.000);
												};
												object("Bip001 L Finger12")
												{
													transform
													{
														position(-0.076, 0.000, 0.000);
														rotation(0.000, 0.000, 346.631);
														scale(1.000, 1.000, 1.000);
													};
												};
											};
										};
									};
								};
							};
						};
						object("Bip001 R Clavicle")
						{
							transform
							{
								position(0.031, -0.029, -0.274);
								rotation(344.473, 59.185, 189.072);
								scale(1.000, 1.000, 1.000);
							};
							object("Bip001 R UpperArm")
							{
								transform
								{
									position(-0.296, 0.000, 0.000);
									rotation(355.476, 9.302, 352.846);
									scale(1.000, 1.000, 1.000);
								};
								object("Bip001 R Forearm")
								{
									transform
									{
										position(-0.713, 0.000, 0.000);
										rotation(0.000, 0.000, 12.381);
										scale(1.000, 1.000, 1.000);
									};
									object("Bip001 R Hand")
									{
										transform
										{
											position(-0.554, 0.000, 0.000);
											rotation(79.168, 199.338, 190.741);
											scale(1.000, 1.000, 1.000);
										};
										object("Bip001 R Finger0")
										{
											transform
											{
												position(-0.282, 0.025, 0.174);
												rotation(311.854, 79.222, 327.445);
												scale(1.000, 1.000, 1.000);
											};
											object("Bip001 R Finger01")
											{
												transform
												{
													position(-0.155, 0.000, 0.000);
													rotation(0.000, 0.000, 359.064);
													scale(1.000, 1.000, 1.000);
												};
												object("Bip001 R Finger02")
												{
													transform
													{
														position(-0.091, 0.000, 0.000);
														rotation(0.000, 0.000, 340.918);
														scale(1.000, 1.000, 1.000);
													};
												};
											};
										};
										object("Bip001 R Finger1")
										{
											transform
											{
												position(-0.531, 0.000, 0.012);
												rotation(359.504, 354.585, 348.463);
												scale(1.000, 1.000, 1.000);
											};
											object("Bip001 R Finger11")
											{
												transform
												{
													position(-0.102, 0.000, 0.000);
													rotation(0.000, 0.000, 340.529);
													scale(1.000, 1.000, 1.000);
												};
												object("Bip001 R Finger12")
												{
													transform
													{
														position(-0.076, 0.000, 0.000);
														rotation(0.000, 0.000, 346.631);
														scale(1.000, 1.000, 1.000);
													};
												};
											};
										};
									};
								};
							};
						};
					};
				};
			};
		};
		object("Bip001 Prop1")
		{
			transform
			{
				position(-0.094, -1.780, 0.247);
				rotation(0.000, 271.801, 0.000);
				scale(1.000, 1.000, 1.000);
			};
		};
	};
	object("Object01")
	{
		transform
		{
			position(1.779, 2.250, 0.144);
			rotation(270.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
		};
		component("SkinnedMeshRenderer", "UnityEngine.SkinnedMeshRenderer, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			mesh("Object01", "Assets/ThirdParty/FantasyMonster/Skeleton/Character/Skeleton@Skin.FBX");
			rootbone("Bip001 Prop1");
			material("skeleton_D", "Assets/ThirdParty/FantasyMonster/Skeleton/Character/Materials/skeleton_D.mat");
		};
	};
	object("Object02")
	{
		transform
		{
			position(0.000, 0.000, 0.000);
			rotation(270.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
		};
		component("SkinnedMeshRenderer", "UnityEngine.SkinnedMeshRenderer, UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
		{
			mesh("Object02", "Assets/ThirdParty/FantasyMonster/Skeleton/Character/Skeleton@Skin.FBX");
			rootbone("Bip001 Pelvis");
			material("skeleton_D", "Assets/ThirdParty/FantasyMonster/Skeleton/Character/Materials/skeleton_D.mat");
		};
	};
	object("bloodbar")
	{
		transform
		{
			position(0.000, 5.210, 0.000);
			rotation(0.000, 0.000, 0.000);
			scale(1.000, 1.000, 1.000);
		};
	};
};
