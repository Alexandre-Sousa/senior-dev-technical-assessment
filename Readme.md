## Part 1 – Java Snippet

1. O primeiro erro, e talvez o mais gritante, consta no laço ***for***. Ele está fazendo a leitura do arquivo ***data.txt*** 10 vezes para fazer, em cada uma delas, a mesma operação: ler linha por linha do arquivo até seu fim (***EOF***), enquanto converte cada uma delas em letras maiúsculas e adiciona à lista ***lines***. Não vejo razão para isso e gasta recurso de processamento no servidor.

2. A lista ***lines*** está sendo declarada e iniciada da seguinte forma:
***private static List<String> lines = new ArrayList<>();***
Porém, ***ArrayList*** não é thread-safe. Isso significa que, caso várias threads estejam trabalhando em cima dessa variável, pode haver corrupção de dados.
Neste caso, é recomendado usar o método ***synchronizedList*** da classe ***Collections*** (vide documento https://docs.oracle.com/javase/8/docs/api/java/util/ArrayList.html da própria Oracle).

3. Ao invés de usar o ***try-catch*** no trecho ***BufferedReader br = new BufferedReader(new FileReader("data.txt"));***, seria melhor usar o ***try-with-resources***, passando como parâmetro esse mesmo trecho. Isso garantiria o fechamento automático, caso ocorresse algum erro antes do ***close()***.

4. Não se trata de um erro, porém, caso seja importante saber a quantidade de linhas processadas e imprimir o resultado no log do servidor (comando ***System.out.println***), deve-se usar o método ***awaitTermination***, da própria interface ***ExecutorService***, passando como parâmetros um ***long*** como timeout e a unidade de tempo. Isso permitirá um tempo extra para aguardar o término das threads que ainda estão em execução. Essa linha pode ser adicionada depois de ***executor.shutdown();*** e antes de ***System.out.println("Lines processed: " + lines.size());*** (vide documento https://docs.oracle.com/javase/8/docs/api/java/util/concurrent/ExecutorService.html).



## Part 2 – C# Snippet

1. Está sendo declarada e instanciada a classe ***HttpClient*** em cada uma das chamadas do ***DownloadAsync***. Isso pode sobrecarregar o serviço. Uma melhor prática seria deixar essa instância global e reutilizá-la.

2. São feitas diversas adições à lista ***cache***, o que pode ocorrer em concorrência e, por consequência, pode causar corrupção de dados. Seria uma boa prática instanciar globalmente uma variável para ser usada como chave para um mecanismo ***lock***, permitindo apenas um acesso por vez, e evitando esse conflito. Algo como:
*private static readonly object lockObj = new object();
lock(lockObj){cache.Add(data);}*


3. Como citado no passo 4 do Java Snippet, caso seja importante saber a quantidade de linhas processadas e imprimir o resultado no log do servidor, é interessante usar mecanismo que force o código a aguardar todos os downloads antes de imprimir o total usando o ***cache.Count***.
