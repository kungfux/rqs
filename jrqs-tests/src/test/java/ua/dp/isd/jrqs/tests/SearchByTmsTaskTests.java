package ua.dp.isd.jrqs.tests;

import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;
import org.junit.Test;

public class SearchByTmsTaskTests {
    
    private WebHelper wh = new WebHelper();
    private final SearchPageHelper searchPage = new SearchPageHelper();
    
    @Test
    public void TestSearchByTmsTask() {
        String searchText = "COBRA-12345";
        String[] expected = { "COBRA-12345, 12346", "COBRA-12345, COBRA-12347" };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 3);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhIsCaseInsensitive() {
        String searchText = "cobra-12345";
        String[] expected = { "COBRA-12345, 12346", "COBRA-12345, COBRA-12347" };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 3);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhBySeveralTmsTasks() {
        String searchText = "COBRA-12345,COBRA-12347";
        String[] expected = { "COBRA-12345, COBRA-12347" };
        String[] actual = searchPage.doSearchByAndGetCellResults(searchText, 3);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhByTmsTaskByClickOnIt() {
        String searchText = "FR200,FR1";
        int firstResultsCount = searchPage.doSearchByAndGetCellResults(searchText, 3).length;
        assertEquals(firstResultsCount, 2);
        
        wh.findElement("table tr:nth-child(2) td:nth-child(3) > a").click();
        int secondResultsCount = searchPage.getSearchResultsByColumn(3).length;
        assertEquals(secondResultsCount, 1);
    }
}
