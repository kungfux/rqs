package ua.dp.isd.jrqs.tests;

import static org.junit.Assert.assertArrayEquals;
import org.junit.Test;

public class SearchByTextTests {
    
    private WebHelper wh = new WebHelper();
    private final SearchPageHelper searchPage = new SearchPageHelper();
    
    @Test
    public void TestSeacrhByText() {
        String searchText = "empty cells";
        String[] expected = { "Requirement with empty cells." };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 5);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhByTextIsCaseInsensitive() {
        String searchText = "EMPTY CELLS";
        String[] expected = { "Requirement with empty cells." };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 5);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhBySeveralKeywords() {
        String searchText = "us,date,general";
        String[] expected = { "US date in general cell format with dot as delimiter" };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 5);
        assertArrayEquals(expected, actual);
    }
}
