package ua.dp.isd.jrqs.tests.searchbox;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;
import org.junit.Test;

public class SearchBySourceTests {
    
    private final SearchPageHelper sp = new SearchPageHelper();
    
    @Test
    public void TestSearchResultsMayBeLimitedBySource() {
        sp.doSearchBy("FR1,FR2");       
        sp.clickToFilterOutBySourceInRow(2);
        String[] actual = sp.getSearchResultsInColumn(2);
        assertEquals(1, actual.length);
        assertEquals("FR2", actual[0]);
    }
    
    @Test
    public void TestSearchResultsCannotBeLimitedBySourceIfAlreadyLimited() {
        sp.doSearchBy("FR1,FR2");
        Boolean isSourceLinkExistsAfterSearch = sp.isFilterOutBySourceLinksAvailable();
        assertTrue(isSourceLinkExistsAfterSearch);
        sp.clickToFilterOutBySourceInRow(2);
        Boolean isSourceLinkExistsAfterLimiting = sp.isFilterOutBySourceLinksAvailable();
        assertFalse(isSourceLinkExistsAfterLimiting);
    }
}
