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
	
	$playerFolder = $_POST["playerFolder"];	
	
	$playerFolderPath = '../Data/'.$playerFolder.'/';

	if (!is_dir($playerFolderPath))
	{
		if(mkdir($playerFolderPath) !== FALSE)
		{
			echo "Folder Created";
		}
		else
		{
			echo "Error Creating Folder";
		}		
	}
	else
	{
		echo "Folder Already Exists";
	}	

?>