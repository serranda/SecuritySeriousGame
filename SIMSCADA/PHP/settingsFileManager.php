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
	
	$folderName = $_POST["folderName"];
	$settingsFolder = $_POST["settingsFolder"];
	$settingFileName = $_POST["settingFileName"];
	
	$settingsFolderPath = '../PlayerData/'.$folderName.'/'.$settingsFolder.'/';
	$settingsFilePath = $settingsFolderPath.$settingFileName;
	
	
	//read file content and send to unity
	if($mode == 'r')
	{
		if(!file_get_contents($settingsFilePath))
		{
			echo "Error Reading File";		

			if (!is_dir($settingsFolderPath))
			{
				mkdir($settingsFolderPath);			
			}
		}
		else
		{
			$file = file_get_contents($settingsFilePath);
			echo $file;
		}
	}
	
	//write settings value into file
	else
	{
		
		$settingsContent = $_POST["settingsContent"];

		if (file_put_contents($settingsFilePath, $settingsContent) !== false) 
		{
			echo "File updated (" . basename($settingsFilePath) . ").";		
		} 
		else 
		{
			echo "Cannot update the file (" . basename($settingsFilePath) . ").";
		}
	}
	

	

?>