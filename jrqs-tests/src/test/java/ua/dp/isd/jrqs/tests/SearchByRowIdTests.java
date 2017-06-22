package ua.dp.isd.jrqs.tests;

import static org.junit.Assert.assertEquals;
import org.junit.Test;

public class SearchByRowIdTests {
    
    private WebHelper wh = new WebHelper();
    private final SearchPageHelper searchPage = new SearchPageHelper();
    
    @Test
    public void TestConcreteFRMayBeOpened() {
        String searchText = "FR1,FR2";
        int firstResultsCount = searchPage.doSearchByAndGetCellResults(searchText, 2).length;
        assertEquals(searchText.split(",").length, firstResultsCount);
        
        wh.findElement("a[title='Share']").click();
        int secondResultsCount = searchPage.getSearchResultsByColumn(2).length;
        assertEquals(1, secondResultsCount);
    }
}
