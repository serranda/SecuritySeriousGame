<?php

    header('Access-Control-Allow-Origin: *');
    header("Access-Control-Allow-Credentials: true");
    header('Access-Control-Allow-Methods: GET, PUT, POST, DELETE, OPTIONS');
    header('Access-Control-Max-Age: 1000');
    header('Access-Control-Allow-Headers: Content-Type, Content-Range, Content-Disposition, Content-Description');
?>
<?php

	error_reporting(0);
	ini_set('display_errors', 0);
	
	
	$mode = $_POST["mode"];
	
	$mainDataFolder = $_POST["mainDataFolder"];
	$playerFolder = $_POST["playerFolder"];
	$settingsFolder = $_POST["settingsFolder"];
	$settingFileName = $_POST["settingFileName"];
	
	$playerSettingsFolderPath = '../'.$mainDataFolder.'/'.$playerFolder.'/'.$settingsFolder.'/';
	$playerSettingsFilePath = $playerSettingsFolderPath.$settingFileName;
	
	
	//read file content and send to unity
	if($mode == 'r')
	{
		if(file_get_contents($playerSettingsFilePath) !== FALSE)
		{
			$file = file_get_contents($playerSettingsFilePath);
			echo $file;

		}
		else
		{			
			echo "Error Reading File";		

			if (!is_dir($playerSettingsFolderPath))
			{
				mkdir($playerSettingsFolderPath);			
			}
		}
	}	
	//write settings value into file
	else
	{		
		$settingsContent = $_POST["settingsContent"];

		if (file_put_contents($playerSettingsFilePath, $settingsContent) !== FALSE) 
		{
			echo "File updated (" . basename($playerSettingsFilePath) . ").";		
		} 
		else 
		{
			echo "Cannot update the file (" . basename($playerSettingsFilePath) . ").";
		}
	}

?>