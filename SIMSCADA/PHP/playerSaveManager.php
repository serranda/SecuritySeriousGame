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
	$saveFolder = $_POST["saveFolder"];
	$saveFileName = $_POST["saveFileName"];
	
	$playerSaveFolderPath = '../'.$mainDataFolder.'/'.$playerFolder.'/'.$saveFolder.'/';
	$playerSaveFilePath = $playerSaveFolderPath.$saveFileName;
	
	
	//read file content and send to unity
	if($mode == 'r')
	{
		if(file_get_contents($playerSaveFilePath) !== FALSE)
		{
			$file = file_get_contents($playerSaveFilePath);
			echo $file;
		}
		else
		{			
			echo "Error Reading File";		

			if (!is_dir($playerSaveFolderPath))
			{
				mkdir($playerSaveFolderPath);			
			}
		}
	}	
	//delete gamedata file from server
	else if ($mode == 'd')
	{
		if(file_get_contents($playerSaveFilePath) !== FALSE)
		{
			unlink(realpath($playerSaveFilePath));
			echo "File Deleted";
		}
		else
		{			
			echo "Error Deleting File";		

			if (!is_dir($playerSaveFolderPath))
			{
				mkdir($playerSaveFolderPath);			
			}
		}
	}
	//write gamedata value into file
	else
	{		
		$saveContent = $_POST["saveContent"];
					
		if (!is_dir($playerSaveFolderPath))
		{
			mkdir($playerSaveFolderPath);			
		}

		if (file_put_contents($playerSaveFilePath, $saveContent) !== FALSE) 
		{
			echo "File updated (" . basename($playerSaveFilePath) . ").";		
		} 
		else 
		{
			echo "Cannot update the file (" . basename($playerSaveFilePath) . ").";
		}
	}

?>