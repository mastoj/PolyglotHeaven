class elasticsearch {

	windowsfunctions::port { 'elasticsearch':
		number    => 9200,
		enableTcp => true,
	}

	nirvanaservice::service {'elasticsearch': 
		config			=> "puppet:///modules/elasticsearch/elasticsearch-1.3.4.json",
		ensure          => '1.3.4',
		pkgName         => 'elasticsearch',
		source          => 'https://www.myget.org/F/crazy-choco/',
		install_options => ['-pre'],
		require			=> Package['java-server-jre']
	}

	define plugin() {
		$programdata = $::programdata
		$version = '1.3.4'
		notify { "elasticsearch plugin $name": }
		exec { "install_$name":
			require   => Nirvanaservice::Service['elasticsearch'],
			cwd       => "$programdata/elasticsearch/elasticsearch-$version",
			command   => "(bin/plugin.bat -i elasticsearch/$name/latest); (nirvanaservice stop -servicename:elasticsearch); (nirvanaservice start -servicename:elasticsearch) ",
			onlyif    => "((./bin/plugin.bat -l) | where {\$_.Contains('$name')}).Length -eq 0",
			logoutput => true,
			provider  => powershell,
		}
	}
}