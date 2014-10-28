class nirvanaservice {
	package { 'nirvanaservice':
		ensure => $version,
		source => 'https://www.myget.org/F/crazy-choco/',
		install_options => ['-pre'],
	}

	define service ($ensure, $config, $source, $pkgName, $install_options, $configmode = '0774') {
		$nirvanaservicename = 'nirvanaservice'

		if $ensure == 'absent' {
			exec {"uninstall_service_$name":
				command => "& nirvanaservice uninstall -servicename:$name",
				onlyif => [
					'powershell (Get-Service | Where-Object {$_.Name -eq "VSS"}).Length -eq 0'
				],
			}

			package { $pkgName:
				ensure          => $version,
				source          => $source,
				require         => Exec["uninstall_service_$name"],
				install_options => $install_options,
			}
		}
		else {
			$choco = $::chocolateyinstall
			$configFile = "$choco/lib/nirvanaservice.1.0.0/tools/conf/$name.json"

			package { $pkgName:
				ensure => $version,
				source => $source,
				install_options => ['-pre'],
			}

			file { $configFile:
				source             => $config,
				mode   	           => $configmode,
				source_permissions => ignore,
				require            => Package[$nirvanaservicename],
				notify             => Service[$name],
			}

			exec {"install_service_$name":
				command => "& nirvanaservice install -servicename:$name",
				onlyif  => [
					'powershell (Get-Service | Where-Object {$_.Name -eq "VSS"}).Length -eq 0'
				],
				require => [Package[$name], Package[$nirvanaservicename]],
			}

			service { $name:
				ensure  => 'running',
				enable  => true,
				require => [File[$configFile], Exec["install_service_$name"]],
			}
		}
	}
}
