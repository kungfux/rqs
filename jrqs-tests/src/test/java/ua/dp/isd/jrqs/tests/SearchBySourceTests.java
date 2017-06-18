package ua.dp.isd.jrqs.tests;

import static org.junit.Assert.assertEquals;
import org.junit.Test;

public class SearchBySourceTests {
    
    private WebHelper wh = new WebHelper();
    private final SearchPageHelper searchPage = new SearchPageHelper();
    
    @Test
    public void TestSearchResultsMayBeLimitedBySource() {
        String searchText = "FR1,FR2";
        int firstResultsCount = searchPage.doSearchByAndGetCellResults(searchText, 2).length;
        assertEquals(firstResultsCount, 2);
        
        wh.findElement("table td:nth-child(12) a").click();
        int secondResultsCount = searchPage.getSearchResultsByColumn(2).length;
        assertEquals(secondResultsCount, 1);
    }
    
    @Test
    public void TestSearchResultsCannotBeLimitedBySourceIfAlreadyLimited() {
        String searchText = "FR1,FR2";
        int firstResultsCount = searchPage.doSearchByAndGetCellResults(searchText, 2).length;
        assertEquals(firstResultsCount, 2);
        
        wh.findElement("table td:nth-child(12) a").click();
        int secondResultsCount = searchPage.getSearchResultsByColumn(2).length;
        assertEquals(secondResultsCount, 1);
        
        int linksCount = wh.findElements("table td:nth-child(12) a").size();
        assertEquals(linksCount, 0);
    }
}
