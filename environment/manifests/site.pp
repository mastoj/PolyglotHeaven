if $::kernel == windows {
  # default package provider
  Package { provider => chocolatey }
  Exec { provider => powershell }
}

import "nodes.pp"
