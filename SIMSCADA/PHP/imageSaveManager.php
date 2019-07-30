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
	
	$playerFolder = $_POST["playerFolder"];
	$saveFolder = $_POST["saveFolder"];
	$imageFileName = basename($_FILES['imageFile']['name']);
	
	$playerSaveFolderPath = '../Data/'.$playerFolder.'/'.$saveFolder.'/';
	$imageSaveFilePath = $playerSaveFolderPath.$imageFileName;
	
	
	//read file content and send to unity
	if($mode == 'r')
	{
		if(file_get_contents($imageSaveFilePath) !== FALSE)
		{
			$file = file_get_contents($imageSaveFilePath);
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
	//write settings value into file
	else
	{		
		//$imageFile = $_FILES["imageFile"];
					
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