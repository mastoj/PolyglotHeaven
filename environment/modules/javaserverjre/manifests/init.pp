class javaserverjre {
	package { 'java-server-jre':
		ensure          => '1.7.0-u67',
		source          => 'https://www.myget.org/F/crazy-choco/',
		install_options => ['-pre'],
	}
}