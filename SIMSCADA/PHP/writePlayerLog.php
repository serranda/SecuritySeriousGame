<?php

    header('Access-Control-Allow-Origin: *');
    header("Access-Control-Allow-Credentials: true");
    header('Access-Control-Allow-Methods: GET, PUT, POST, DELETE, OPTIONS');
    header('Access-Control-Max-Age: 1000');
    header('Access-Control-Allow-Headers: Content-Type, Content-Range, Content-Disposition, Content-Description');
?>
<?php
	
	$folderName = $_POST["folderName"];
	$fileName = $_POST["fileName"];
	$fileContent = $_POST["fileContent"];
	
	$filePath = '../PlayerData/'.$folderName.'/'.$fileName.'.log';

	if (file_put_contents($filePath, $fileContent, FILE_APPEND) !== false) 
	{
		echo "File created (" . basename($filePath) . ").";
		
	} 
	else 
	{
		echo "Cannot create the file (" . basename($filePath) . ").";
	}
	

?>