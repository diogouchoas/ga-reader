# Google Analytics Reader

Aplicação de linha de comando capaz de se autenticar por OAuth2 e coletar dados da API do Google Analytics.

ga-reader.exe --help

ga-reader 1.0.0.0
Copyright ©  2015

  q          Query

  r          Reset

  a          Authorize

  help       Display more information on a specific command.

  version    Display version information.


ga-reader.exe q --help
ga-reader 1.0.0.0
Copyright ©  2015

  --onlytotals        (Default: false) Mostrar somente o total.

  -t, --tableid       Required. TableId. Ex: ga:21810642

  -m, --metrics       Required. Métricas. Ex: rt:activeUsers

  -d, --dimensions    Dimensões. Ex: rt:city

  --help              Display this help screen.

  --version           Display version information.

Exemplo:

  ga-reader.exe q -t ga:21810642 -d rt:city -m rt:activeUsers --onlytotals
  ga-reader.exe q -t ga:21810642 -m rt:activeUsers --onlytotals
  ga-reader.exe q -t ga:21810642 -m rt:activeUsers


