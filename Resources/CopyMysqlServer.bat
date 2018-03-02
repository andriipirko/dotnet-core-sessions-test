Xcopy /E /H "%~dp0\MySQL\*" "C:\MySQL\*"
cd /d "C:\MySQL\bin"
mysqld -install
net start mysql