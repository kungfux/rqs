package ua.dp.isd.jrqs.tests.searchbox;

import static org.junit.Assert.assertArrayEquals;
import org.junit.Test;

public class SearchByTextTests {
    
    private final SearchPageHelper sp = new SearchPageHelper();
    
    @Test
    public void TestSeacrhByText() {
        String[] expected = { "Requirement with empty cells." };
        sp.doSearchBy("empty cells");
        String[] actual = sp.getSearchResultsInColumn(5);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhByTextIsCaseInsensitive() {
        String[] expected = { "Requirement with empty cells." };
        sp.doSearchBy("EMPTY CELLS");
        String[] actual = sp.getSearchResultsInColumn(5);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhBySeveralKeywords() {
        String[] expected = { "US date in general cell format with dot as delimiter" };
        sp.doSearchBy("us,date,general");
        String[] actual = sp.getSearchResultsInColumn(5);
        assertArrayEquals(expected, actual);
    }
}
