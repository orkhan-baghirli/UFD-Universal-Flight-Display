using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine;
using System.Text.RegularExpressions;

public class DATA_PARSING : MonoBehaviour {
	public InputField InputField;
	private string InputString, XString, YString, ZString, X_ROT_String, X_LOC_ROT_String, Forw_Spped_String, Altitude_String, HDG_DEG_String,
		STR_ENG_PWR, ENG_PWR, VRTCL_SPD_String, Angle_Roll_String, ACCELERATION_STRING, Throttle_String, MaxEnginePower_String;
	public float  LOC_ROT_ANG_X, LOC_ROT_ANG_Y, LOC_ROT_ANG_Z, angle_pitch, LOC_ROT_X, ForwardSpeed, Altitude, hdgdeg, 
		StarterEnginePower, EnginePower, acceleration, vertical_speed, angle_roll, Throttle, maxEnginePower;

	void Update ()
	{
		InputString = InputField.text.Replace(',', '.');
		XString = " ";   YString = " ";   ZString = " ";  X_ROT_String = " "; X_LOC_ROT_String = " "; Forw_Spped_String = " ";
		Altitude_String = " "; HDG_DEG_String = " "; STR_ENG_PWR = " "; ENG_PWR = " "; VRTCL_SPD_String = " "; Angle_Roll_String = " "; 
		ACCELERATION_STRING = " "; Throttle_String = " "; MaxEnginePower_String = " "; 
		int z = 0;
		if (InputString != "")
		{
			while (InputString[z] != 'a')
			{
				XString += InputString[z];
				z++;
			};

			while (InputString[z + 1] != 'b')
			{
				YString += InputString[z + 1];
				z++;
			};

			while (InputString[z + 2] != 'c')
			{
				ZString += InputString[z + 2];
				z++;
			};
			
			while (InputString[z + 3] != 'd')
			{
				X_ROT_String += InputString[z + 3];
				z++;
			};

			while (InputString[z + 4] != 'e')
			{
				Forw_Spped_String += InputString[z + 4];
				z++;
			};

			while (InputString[z + 5] != 'f')
			{
				Altitude_String += InputString[z + 5];
				z++;
			};
			
			while (InputString[z + 6] != 'h')
			{
				HDG_DEG_String += InputString[z + 6];
				z++;
			};
			
			while (InputString[z + 7] != 'g')
			{
				STR_ENG_PWR += InputString[z + 7];
				z++;
			};
						
			while (InputString[z + 8] != 'i')
			{
				ENG_PWR += InputString[z + 8];
				z++;
			};
			
			while (InputString[z + 9] != 'k')
			{
				X_LOC_ROT_String += InputString[z + 9];
				z++;
			};
						
			while (InputString[z + 10] != 'l')
			{
				ACCELERATION_STRING += InputString[z + 10];
				z++;
			};
						
			while (InputString[z + 11] != 'm')
			{
				VRTCL_SPD_String += InputString[z + 11];
				z++;
			};
						
			while (InputString[z + 12] != 'n')
			{
				Angle_Roll_String += InputString[z + 12];
				z++;
			};
						
			while (InputString[z + 13] != 'o')
			{
				Throttle_String += InputString[z + 13];
				z++;
			};
						
			while (InputString[z + 14] != 'p')
			{
				MaxEnginePower_String += InputString[z + 14];
				z++;
			};
		}

		float.TryParse(XString, NumberStyles.Any, new CultureInfo("en-US"), out LOC_ROT_ANG_X);
		float.TryParse(YString, NumberStyles.Any, new CultureInfo("en-US"),  out LOC_ROT_ANG_Y);
		float.TryParse(ZString, NumberStyles.Any, new CultureInfo("en-US"), out LOC_ROT_ANG_Z);
		float.TryParse(X_ROT_String, NumberStyles.Any, new CultureInfo("en-US"), out angle_pitch);
		float.TryParse(Forw_Spped_String, NumberStyles.Any, new CultureInfo("en-US"), out ForwardSpeed);
		float.TryParse (Altitude_String, NumberStyles.Any, new CultureInfo("en-US"), out Altitude);
		float.TryParse (HDG_DEG_String, NumberStyles.Any, new CultureInfo("en-US"), out hdgdeg);
		float.TryParse (STR_ENG_PWR, NumberStyles.Any, new CultureInfo("en-US"), out StarterEnginePower);
		float.TryParse (ENG_PWR, NumberStyles.Any, new CultureInfo("en-US"), out EnginePower);
		float.TryParse (X_LOC_ROT_String, NumberStyles.Any, new CultureInfo("en-US"), out LOC_ROT_X);
		float.TryParse (ACCELERATION_STRING, NumberStyles.Any, new CultureInfo("en-US"), out acceleration);
		float.TryParse (VRTCL_SPD_String, NumberStyles.Any, new CultureInfo("en-US"), out vertical_speed);
		float.TryParse (Angle_Roll_String, NumberStyles.Any, new CultureInfo("en-US"), out angle_roll);
		float.TryParse (Throttle_String, NumberStyles.Any, new CultureInfo("en-US"), out Throttle);
		float.TryParse (MaxEnginePower_String, NumberStyles.Any, new CultureInfo("en-US"), out maxEnginePower);
	}
}
