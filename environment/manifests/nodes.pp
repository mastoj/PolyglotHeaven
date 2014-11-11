node default {
	notify { 'YOLO': }
}

node 'polyglot' {
	include windowsfunctions
	include nirvanaservice
	include eventstore
	include javaserverjre
	include elasticsearch
	include neo4j

	elasticsearch::plugin { 'marvel': }
	package { 'procexp':
		ensure => '15.13',
	}
}
