package ua.dp.isd.jrqs.tests.searchbox;

import static org.junit.Assert.assertEquals;
import org.junit.Test;

public class SearchByRowIdTests {
    
    private final SearchPageHelper sp = new SearchPageHelper();
    
    @Test
    public void TestConcreteFRMayBeOpened() {
        sp.doSearchBy("FR1,FR2");        
        sp.clickToShareTheRequirementRow(2);
        String[] actual = sp.getSearchResultsInColumn(2);
        assertEquals(1, actual.length);
        assertEquals("FR2", actual[0]);
    }
}
