<?php

    header('Access-Control-Allow-Origin: *');
    header("Access-Control-Allow-Credentials: true");
    header('Access-Control-Allow-Methods: GET, PUT, POST, DELETE, OPTIONS');
    header('Access-Control-Max-Age: 1000');
    header('Access-Control-Allow-Headers: Content-Type, Content-Range, Content-Disposition, Content-Description');
?>
<?php
	
	$mainDataFolder = $_POST["mainDataFolder"];
	$playerFolder = $_POST["playerFolder"];
	$saveFolder = $_POST["saveFolder"];
	$imageFileName = $_POST["imageFileName"];
	
	$playerSaveFolderPath = '../'.$mainDataFolder.'/'.$playerFolder.'/'.$saveFolder.'/';
	$imageSaveFilePath = $playerSaveFolderPath.$imageFileName;	

	if(file_get_contents($imageSaveFilePath) !== FALSE)
	{
		$stat = stat($imageSaveFilePath);
		echo $stat['mtime'];
	}
	else
	{
		echo "Error Reading File";		
	}
?>