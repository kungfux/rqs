package ua.dp.isd.jrqs.tests.searchbox;

import static org.junit.Assert.assertArrayEquals;
import org.junit.Test;

public class SearchByFRIDTests {

    private final SearchPageHelper sp = new SearchPageHelper();
    
    @Test
    public void TestSeacrhByFRID() {
        String[] expected = { "FR1" };
        sp.doSearchBy(String.join("", expected));
        String[] actual = sp.getSearchResultsInColumn(2);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhByNFRID() {
        String[] expected = { "NFR1" };
        sp.doSearchBy(String.join("", expected));
        String[] actual = sp.getSearchResultsInColumn(2);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhIsCaseInsensitive() {
        String[] expected = { "FR1" };
        sp.doSearchBy(String.join("", expected).toLowerCase());
        String[] actual = sp.getSearchResultsInColumn(2);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhBySeveralFRIDs() {
        String[] expected = { "FR1", "FR2" };
        sp.doSearchBy(String.join(",", expected));
        String[] actual = sp.getSearchResultsInColumn(2);
        assertArrayEquals(expected, actual);
    }
    
    @Test
    public void TestSeacrhBySeveralFRIDsWithExtraSpaces() {
        String[] expected = { "FR1", "FR2" };
        sp.doSearchBy(" fr1 , fr2, ");
        String[] actual = sp.getSearchResultsInColumn(2);
        assertArrayEquals(expected, actual);
    }
}
