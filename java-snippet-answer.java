public class FileProcessor {

	private static List<String> lines = Collections.synchronizedList(new ArrayList<>());

	public static void main(String[] args) throws Exception {

	    ExecutorService executor = Executors.newFixedThreadPool(5);

	    try (BufferedReader br = new BufferedReader(new FileReader("data.txt"))) {

	        String line;

	        while ((line = br.readLine()) != null) {

	            final String content = line;
	            executor.submit(() -> lines.add(content.toUpperCase()));
	            
	        }

	    }

	    executor.shutdown();
	    executor.awaitTermination(1, TimeUnit.MINUTES);
	    System.out.println("Lines processed: " + lines.size());

	}

}