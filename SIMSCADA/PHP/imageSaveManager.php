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
	$imageFileName = $_POST["imageFileName"];
	
	$playerSaveFolderPath = '../'.$mainDataFolder.'/'.$playerFolder.'/'.$saveFolder.'/';
	$imageSaveFilePath = $playerSaveFolderPath.$imageFileName;	
	
	//check if image exists and send url to server
	if($mode == 'r')
	{
		if(file_get_contents($imageSaveFilePath) !== FALSE)
		{
			$imageURL = $_SERVER["SERVER_NAME"].'/TestLoginBuildWebGL/Data/'.$playerFolder.'/'.$saveFolder.'/'.$imageFileName;

			echo $imageURL;
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
	//delete image from server
	else if ($mode == 'd')
	{
		if(file_get_contents($imageSaveFilePath) !== FALSE)
		{
			unlink(realpath($imageSaveFilePath));
			echo "Image Deleted";
		}
		else
		{			
			echo "Error Deleting Image";		

			if (!is_dir($playerSaveFolderPath))
			{
				mkdir($playerSaveFolderPath);			
			}
		}
	}
	//upload image to server
	else
	{				
		if (!is_dir($playerSaveFolderPath))
		{
			mkdir($playerSaveFolderPath);			
		}

		if (move_uploaded_file($_FILES['imageFile']['tmp_name'], $imageSaveFilePath) !== FALSE) 
		{
			echo "File uploaded (" . $imageSaveFilePath . ").";		
		} 
		else 
		{
			echo "Cannot upload the file (" . $imageSaveFilePath . ").";
		}
	}

?>