<?php

    header('Access-Control-Allow-Origin: *');
    header("Access-Control-Allow-Credentials: true");
    header('Access-Control-Allow-Methods: GET, PUT, POST, DELETE, OPTIONS');
    header('Access-Control-Max-Age: 1000');
    header('Access-Control-Allow-Headers: Content-Type, Content-Range, Content-Disposition, Content-Description');
?>
<?php
	
	$folderName = $_POST["folderName"];	
	
	$folderPath = '../PlayerData/'.$folderName.'/';

	if (!is_dir($folderPath))
	{
		mkdir($folderPath);
		echo("Folder " . basename($folderPath) . " Created \r\n");
	}
	else
	{
		echo("Folder " . basename($folderPath) . " Already Exists \r\n");
	}	

?>