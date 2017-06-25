package ua.dp.isd.jrqs.tests.searchbox;

import java.util.Arrays;
import static org.junit.Assert.assertArrayEquals;
import org.junit.Test;

public class SearchByRangeOfFRIDTests {

    private final SearchPageHelper sp = new SearchPageHelper();

    @Test
    public void TestSeacrhByRangeOfFRID() {
        String[] expected = {"FR100", "FR101", "FR102"};
        sp.doSearchBy(String.format("%s-%s", expected[0], expected[2]));
        String[] actual = sp.getSearchResultsInColumn(2);
        assertArrayEquals(expected, actual);
    }

    @Test
    public void TestSeacrhByRangeOfFRIDHandlesDirection() {
        String[] expected = {"FR100", "FR101", "FR102"};
        sp.doSearchBy(String.format("%s-%s", expected[2], expected[0]));
        String[] actual = sp.getSearchResultsInColumn(2);
        assertArrayEquals(expected, actual);
    }

    @Test
    public void TestSeacrhByRangeOfFRIDMayBeCombined() {
        String[] expected = {"FR100", "FR101", "FR102", "NFR1", "NFR2", "FR1"};
        sp.doSearchBy(String.format("%s,%s-%s,%s-%s",
                expected[5], expected[0], expected[2], expected[3], expected[4]));
        String[] actual = sp.getSearchResultsInColumn(2);
        Arrays.sort(expected);
        Arrays.sort(actual);
        assertArrayEquals(expected, actual);
    }
}
