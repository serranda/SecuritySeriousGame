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
	$logName = $_POST["logName"];
	$logContent = $_POST["logContent"];
	
	$logPath = '../'.$mainDataFolder.'/'.$playerFolder.'/'.$logName.'.log';

	if (file_put_contents($logPath, $logContent, FILE_APPEND) !== false) 
	{
		echo "File created (" . basename($logPath) . ").";
		
	} 
	else 
	{
		echo "Cannot create the file (" . basename($logPath) . ").";
	}
	

?>