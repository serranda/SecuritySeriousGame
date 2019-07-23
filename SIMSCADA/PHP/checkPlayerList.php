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
	
	$playerListPath = '../PlayerData/players.txt';

	if(!file_exists($playerListPath))
	{
		file_put_contents($playerListPath, "");
		echo "File Created";
	}
	else if(file_get_contents($playerListPath)==FALSE)
	{
		echo "File Empty";
	}
	else
	{
		$file = file_get_contents($playerListPath);
		echo $file;		
	}	

?>