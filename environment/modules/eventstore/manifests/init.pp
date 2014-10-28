class eventstore {
	windowsfunctions::port { 'eventstore_2113':
		number    => 2113,
		enableTcp => true,
	}

	windowsfunctions::port { 'eventstore_1113':
		number    => 1113,
		enableTcp => true,
	}

	nirvanaservice::service {'eventstore': 
		config          => "puppet:///modules/eventstore/eventstore.json",
		ensure          => '3.0.0',
		pkgName         => 'eventstore',
		source          => 'https://www.myget.org/F/crazy-choco/',
		install_options => ['-pre'],
	}
}
