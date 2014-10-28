class neo4j {
	windowsfunctions::port { 'neo4j':
		number    => 7474,
		enableTcp => true,
	}

	file { 'C:/ProgramData/neo4j/neo4j-community-2.1.5/conf/neo4j-server.properties':
		source             => "puppet:///modules/neo4j/neo4j-server.properties",
		mode   	           => '0774',
		require            => Package['neo4j'],
		notify             => Service['neo4j'],
		source_permissions => ignore,
	}

	nirvanaservice::service {'neo4j': 
		config			=> "puppet:///modules/neo4j/neo4j-2.1.5.json",
		ensure          => '2.1.5',
		pkgName         => 'neo4j',
		source          => 'https://www.myget.org/F/crazy-choco/',
		install_options => ['-pre'],
		require			=> Package['java-server-jre'], 
	}
}
