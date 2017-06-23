package ua.dp.isd.jrqs.tests.searchbox;

import static org.junit.Assert.assertArrayEquals;
import static org.junit.Assert.assertEquals;
import org.junit.Test;

public class SearchByTmsTaskTests {
    
    private final SearchPageHelper sp = new SearchPageHelper();
    
    @Test
    public void TestSearchByTmsTask() {
        sp.doSearchBy("COBRA-12345");
        String[] expected = { "COBRA-12345, 12346", "COBRA-12345, COBRA-12347" };
        String[] actual = sp.getSearchResultsInColumn(3);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhIsCaseInsensitive() {
        sp.doSearchBy("cobra-12345");
        String[] expected = { "COBRA-12345, 12346", "COBRA-12345, COBRA-12347" };
        String[] actual = sp.getSearchResultsInColumn(3);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhBySeveralTmsTasks() {
        sp.doSearchBy("COBRA-12345,COBRA-12347");
        String[] expected = { "COBRA-12345, COBRA-12347" };
        String[] actual = sp.getSearchResultsInColumn(3);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhByTmsTaskByClickOnIt() {
        sp.doSearchBy("FR1,FR200");       
        sp.clickToRepeatSearchByTmsTaskInRow(2);
        String[] actual = sp.getSearchResultsInColumn(2);
        assertEquals(1, actual.length);
        assertEquals("FR200", actual[0]);
    }
}
